using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Game state
    public Train Train;
    public Crew Crew;
    public Time Time;
    public Dictionary<ResourceDef, int> TurnResolutionResourceChanges;
    public GameState GameState { get; private set; }

    // User input
    public GameUI UI;
    private InputHandler InputHandler;
    public Character SelectedCharacter { get; private set; }

    // Visual state
    private bool IsShowingTileOccupationOverlay;

    // Rules
    private const int PP_KM_WEIGHT = 10000; // The amount of kg 1 PP can transport 1 km in a turn.
    public const int HUMAN_WEIGHT = 80;

    #region Initialize

    private void Awake()
    {
        ResourceManager.ClearCache();

        // Load defs
        if (DefDatabase<ResourceDef>.AllDefs.Count == 0) // Skip if defs already loaded (like this no rebuild is needed when starting game in editor)
        {
            DefDatabaseRegistry.AddAllGlobalDefs();
            DefDatabaseRegistry.ResolveAllReferences();
            DefDatabaseRegistry.OnLoadingDone();
        }
    }

    // Start is called before the first frame update
    void Start()    
    {
        UI = GameObject.Find("GameUI").GetComponent<GameUI>();

        StartGame();
    }

    private void StartGame()
    {
        InputHandler = new InputHandler(this);
        WorldManager.Initialize();

        TurnResolutionResourceChanges = new Dictionary<ResourceDef, int>();
        Time = new Time();

        OutsideWorld.Instance.CreateBackground();
        OutsideWorld.Instance.CreateTracks();

        InitializeStarterTrain();
        InitializeStarterCrew();

        UI.Init(this);

        StartTurn();
    }

    private void InitializeStarterTrain()
    {
        GameObject trainObject = new GameObject("Train");
        Train = trainObject.AddComponent<Train>();
        Train.Init(this);

        Wagon wagon = WagonManager.CreateWagon(WagonLayoutDefOf.Short, WheelsDefOf.WoodenSpokedWheels, WheelsDefOf.WoodenSpokedWheels, FloorDefOf.WoodenFloor, null);
        AddNewFurniture(FurnitureDefOf.HandcarEngine, wagon.GetTile(1,1), Direction.N, isMirrored: false);
        Train.AddWagon(wagon);
    }

    private void InitializeStarterCrew()
    {
        GameObject crewObject = new GameObject("Crew");
        Crew = crewObject.AddComponent<Crew>();
        Crew.Init(this);
        Crew.CreateStarterCrew();
    }

    #endregion

    #region Update

    // Update is called once per frame
    void Update()
    {
        WorldManager.UpdateHoveredObjects();
        InputHandler.HandleInputs();

        int distance = GetTravelDistance();
        if (distance != 0f) OutsideWorld.Instance.MoveWorld(distance);
    }

    #endregion

    #region Turn Planning

    public void StartTurn()
    {
        GameState = GameState.TurnPlanning;
        UI.UpdateTurnResolution();
    }

    public void StartTurnResolution()
    {
        GameState = GameState.TurnResolution;

        Time.IncreaseTime(1);
        UI.UpdateTimeText();

        EndTurnResolution();
    }

    private void EndTurnResolution()
    {
        StartTurn();
    }

    public Furniture AddNewFurniture(FurnitureDef furnitureDef, Tile origin, Direction rotation, bool isMirrored)
    {
        Furniture newFurniture = FurnitureCreator.CreateFurniture(this, furnitureDef, origin, rotation, isMirrored);
        origin.Wagon.Furniture.Add(newFurniture);
        origin.Wagon.UpdateTileOccupation();
        return newFurniture;
    }

    public void SetOperatingMode(Furniture furniture, OperatingMode mode, List<Character> assignedCharacters)
    {
        Debug.Log($"Setting operating mode of {furniture.LabelCap} to {mode.Label}. Assigned crew: {assignedCharacters.ToListString()}.");

        furniture.SetOperatingMode(mode);
        furniture.SetAssignedCharacters(assignedCharacters);

        UpdateTurnResolution();
    }

    #endregion

    #region Turn Resolution

    /// <summary>
    /// Recalculates all expected changes (resources, mood, needs, vitals, climate, etc.) during turn resolution.
    /// </summary>
    private void UpdateTurnResolution()
    {
        TurnResolutionResourceChanges = GetResourceChanges();

        // UI
        UI.UpdateTurnResolution();
    }

    /// <summary>
    /// Returns all resource changes that would be applied when ending the turn.
    /// </summary>
    private Dictionary<ResourceDef, int> GetResourceChanges()
    {
        Dictionary<ResourceDef, int> resourceChanges = new Dictionary<ResourceDef, int>();

        foreach (Furniture furniture in GetFurniture())
        {
            foreach (var res in furniture.GetResourceChanges())
            {
                if (resourceChanges.ContainsKey(res.Key)) resourceChanges[res.Key] += res.Value;
                else resourceChanges.Add(res.Key, res.Value);
            }
        }

        return resourceChanges;
    }

    /// <summary>
    /// Returns the amount of how much a resource changes when ending the turn.
    /// </summary>
    public int GetResourceChange(ResourceDef resource)
    {
        return TurnResolutionResourceChanges.TryGetValue(resource, out int value) ? value : 0; 
    }

    /// <summary>
    /// Returns the full weight of the train when ending the turn (in kg).
    /// </summary>
    public int GetTrainWeight()
    {
        int weight = 0;

        // Wagons
        foreach (Wagon wagon in Train.Wagons) weight += wagon.GetWeight();

        // Crew
        foreach (Character c in Crew.Characters) weight += c.Weight;

        return weight;
    }

    /// <summary>
    /// Get all furniture in the train when ending the turn.
    /// </summary>
    public List<Furniture> GetFurniture()
    {
        List<Furniture> list = new List<Furniture>();
        foreach (Wagon wagon in Train.Wagons) list.AddRange(wagon.Furniture);
        return list;
    }

    /// <summary>
    /// Returns the distance the train travels in the next turn/hour when ending the turn now.
    /// <br/>This amount also represents the kph the train travels during the turn.
    /// </summary>
    public int GetTravelDistance()
    {
        int propulsionPower = TurnResolutionResourceChanges.ContainsKey(ResourceDefOf.PropulsionPower) ? TurnResolutionResourceChanges[ResourceDefOf.PropulsionPower] : 0;
        int weight = GetTrainWeight();

        int distance = propulsionPower * (PP_KM_WEIGHT / weight);
        return distance;
    }

    #endregion

    #region Getters

    

    

    #endregion

    #region Input State

    public void SelectCharacter(Character c)
    {
        DeselectCharacter();
        SelectedCharacter = c;
        SelectedCharacter.Select();
    }

    public void DeselectCharacter()
    {
        SelectedCharacter?.Deselect();
        SelectedCharacter = null;
    }

    #endregion

    #region Visual

    public void RelocateAllBlockedCharacters()
    {
        foreach (Character c in Crew.Characters)
        {
            if (c.CurrentTile.Occupation == TileOccupation.Blocked)
            {
                c.TeleportOnTile(c.CurrentTile.Wagon.GetRandomEmptyTile(), HelperFunctions.GetRandomDirection4());
            }
        }
    }

    public void ToggleTileOccupationOverlay()
    {
        IsShowingTileOccupationOverlay = !IsShowingTileOccupationOverlay;
        Train.ShowTileOccupationOverlay(IsShowingTileOccupationOverlay);
    }

    #endregion
}

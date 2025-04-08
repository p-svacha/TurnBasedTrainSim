using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Game state
    public Train Train;
    public Crew Crew;
    public GameState GameState { get; private set; }

    // User input state
    private InputHandler InputHandler;
    public Character SelectedCharacter { get; private set; }

    // Visual state
    private bool IsShowingTileOccupationOverlay;

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
        StartGame();
    }

    private void StartGame()
    {
        InputHandler = new InputHandler(this);
        WorldManager.Initialize();

        OutsideWorld.Instance.CreateBackground();
        OutsideWorld.Instance.CreateTracks();

        InitializeStarterTrain();
        InitializeStarterCrew();

        GameState = GameState.PlanningPhase;
    }

    private void InitializeStarterTrain()
    {
        GameObject trainObject = new GameObject("Train");
        Train = trainObject.AddComponent<Train>();
        Train.Init(this);

        Wagon wagon = WagonManager.CreateWagon(WagonLayoutDefOf.Short, WheelsDefOf.WoodenSpokedWheels, WheelsDefOf.WoodenSpokedWheels, FloorDefOf.WoodenFloor, null);
        wagon.AddNewFurniture(FurnitureDefOf.HandcarEngine, new Vector2Int(1, 1), Direction.N, isMirrored: false);
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

        if (Train.Speed != 0f) OutsideWorld.Instance.MoveWorld(Train.Speed);
    }

    #endregion

    #region Simulation

    public void SetOperatingMode(Furniture furniture, OperatingMode mode)
    {

    }

    public Dictionary<ResourceDef, int> GetOutputResources()
    {
        Dictionary<ResourceDef, int> outputResources = new Dictionary<ResourceDef, int>();
        return outputResources;
    }

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
                c.TeleportOnTile(c.CurrentTile.Wagon.GetRandomEmptyTile());
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Train Train;
    public Crew Crew;

    public GameObject HoveredObject;

    // Visual
    private bool IsShowingTileOccupationOverlay;

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
        WorldManager.Initialize();

        OutsideWorld.Instance.CreateBackground();
        OutsideWorld.Instance.CreateTracks();

        InitializeStarterTrain();
        InitializeStarterCrew();
    }

    private void InitializeStarterTrain()
    {
        GameObject trainObject = new GameObject("Train");
        Train = trainObject.AddComponent<Train>();

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period)) Train.Speed += 10f;
        if (Input.GetKeyDown(KeyCode.Comma)) Train.Speed -= 10f;
        if (Input.GetKeyDown(KeyCode.O)) ToggleTileOccupationOverlay();

        if (Train.Speed != 0f) OutsideWorld.Instance.MoveWorld(Train.Speed);

        WorldManager.UpdateHoveredObjects();
    }

    private void ToggleTileOccupationOverlay()
    {
        IsShowingTileOccupationOverlay = !IsShowingTileOccupationOverlay;
        Train.ShowTileOccupationOverlay(IsShowingTileOccupationOverlay);
    }
}

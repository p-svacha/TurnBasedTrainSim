using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Train Train;
    public Crew Crew;

    public GameObject HoveredObject;

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
        Train.AddStarterWagon();
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

        if (Train.Speed != 0f) OutsideWorld.Instance.MoveWorld(Train.Speed);

        WorldManager.UpdateHoveredObjects();
    }
}

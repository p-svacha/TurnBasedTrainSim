using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public List<Wagon> Wagons = new List<Wagon>();
    public float Speed;

    public void AddStarterWagon()
    {
        GameObject wagonObject = new GameObject("Wagon");
        wagonObject.transform.SetParent(transform);
        Wagon wagon = wagonObject.AddComponent<Wagon>();
        wagon.InitializeStarterWagon();

        Wagons.Add(wagon);
    }

    public Tile GetRandomTile()
    {
        return Wagons[0].GetRandomTile();
    }
}

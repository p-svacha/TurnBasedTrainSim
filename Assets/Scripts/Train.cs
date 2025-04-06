using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public List<Wagon> Wagons = new List<Wagon>();
    public float Speed;

    public void AddStarterWagon()
    {
        Wagon wagon = WagonManager.CreateWagon(WagonLayoutDefOf.Standard, WheelsDefOf.WoodenSpokedWheels, WheelsDefOf.WoodenSpokedWheels, FloorDefOf.WoodenFloor, null);
        wagon.transform.SetParent(transform);

        Wagons.Add(wagon);
    }

    public Tile GetRandomTile()
    {
        return Wagons[0].GetRandomTile();
    }
}

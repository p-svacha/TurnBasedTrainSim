using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public List<Wagon> Wagons = new List<Wagon>();
    public float Speed;

    public void AddWagon(Wagon wagon)
    {
        wagon.transform.SetParent(transform);
        Wagons.Add(wagon);
    }

    public Tile GetRandomTile()
    {
        return Wagons[0].GetRandomTile();
    }

    public void ShowTileOccupationOverlay(bool value)
    {
        foreach (Wagon wagon in Wagons) wagon.ShowTileOccupationOverlay(value);
    }
}

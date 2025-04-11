using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public Game Game;
    public List<Wagon> Wagons = new List<Wagon>();

    public void Init(Game game)
    {
        Game = game;
    }

    public void AddWagon(Wagon wagon)
    {
        wagon.Train = this;
        wagon.transform.SetParent(transform);
        Wagons.Add(wagon);
    }

    public Tile GetRandomTile()
    {
        return Wagons.RandomElement().GetRandomTile();
    }
    public Tile GetRandomEmptyTile()
    {
        return Wagons.RandomElement().GetRandomEmptyTile();
    }

    public void ShowTileOccupationOverlay(bool value)
    {
        foreach (Wagon wagon in Wagons) wagon.ShowTileOccupationOverlay(value);
    }
}

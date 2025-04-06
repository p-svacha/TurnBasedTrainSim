using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Simulation
    public Job Job;

    // Visual
    public Tile CurrentTile;

    public void TeleportOnTile(Tile tile)
    {
        CurrentTile = tile;
        transform.position = tile.GetWorldPosition();
    }
}

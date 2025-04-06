using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A single tile inside a wagon
/// </summary>
public class Tile
{
    public const float TILE_SIZE = 0.5f;

    /// <summary>
    /// The wagon this tile belongs to.
    /// </summary>
    public Wagon Wagon;

    /// <summary>
    /// Coordinates of the tile within the wagon.
    /// </summary>
    public Vector2Int Coordinates;

    /// <summary>
    /// Defines if furniture can be placed here and if crew can walk here.
    /// </summary>
    public TileOccupation Occupation;

    public Tile(Wagon wagon, Vector2Int coords)
    {
        Wagon = wagon;
        Coordinates = coords;
    }

    public Vector3 GetWorldPosition()
    {
        float x = (Coordinates.x * TILE_SIZE) - (Wagon.Length * 0.5f * TILE_SIZE) + (TILE_SIZE * 0.5f);
        float z = (Coordinates.y * TILE_SIZE) - (Wagon.Width * 0.5f * TILE_SIZE) + (TILE_SIZE * 0.5f);
        Vector3 localPos = new Vector3(x, 0f, z);

        return localPos; // todo: add wagon position
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains information about the position and face direction of an interaction spot.
/// <br/>May be local within furniture or global within wagon.
/// </summary>
public class InteractionSpot
{
    public Vector2Int Position { get; private set; }
    public Direction Direction { get; private set; }

    public InteractionSpot(Vector2Int position, Direction direction)
    {
        Position = position;
        Direction = direction;
    }
    public InteractionSpot(int x, int y, Direction direction)
    {
        Position = new Vector2Int(x, y);
        Direction = direction;
    }

    public Tile GetWagonTile(Wagon wagon) => wagon.GetTile(Position);
}

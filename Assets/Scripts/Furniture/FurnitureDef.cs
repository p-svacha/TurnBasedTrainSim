using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FurnitureDef : Def
{
    /// <summary>
    /// The path after Resources/ leading to the prefab that gets instantiated to represent this furniture.
    /// </summary>
    public abstract string PrefabResourcePath { get; }

    /// <summary>
    /// The size (amount of tiles) the model of this furniture occupies when rotated default (North).
    /// <br/>If not specified by UnoccupiedTiles, all tiles within the dimensions will count as blocked.
    /// <br/>Individual tiles can be set as interaction spot tiles with InteractionSpotTiles, even outside of the dimensions.
    /// </summary>
    public abstract Vector2Int Dimensions { get; }

    /// <summary>
    /// A list of tiles that act are within the dimensions, but not actually occupied.
    /// <br/>The coordinates in the list act as an offset of the origin point.
    /// <br/>Must be within dimensions, useful for non-rectangular shapes.
    /// </summary>
    public virtual List<Vector2Int> UnoccupiedTiles { get; } = new List<Vector2Int>();

    /// <summary>
    /// A list of tiles that act as interaction spots for crew to interact with this furniture.
    /// <br/>The coordinates in the list act as an offset of the origin point.
    /// <br/>May be outside of dimensions.
    /// </summary>
    public virtual List<Vector2Int> InteractionSpotTiles { get; } = new List<Vector2Int>();

    /// <summary>
    /// Returns a list with all tiles that need to be completely free to be able to place this furniture.
    /// <br/>The coordinates in the list act as an offset of the origin point where placed.
    /// </summary>
    public List<Vector2Int> GetRequiredFreeTiles()
    {
        List<Vector2Int> tiles = new List<Vector2Int>();

        // Add all not-unoccupied tiles within dimensions
        for(int x = 0; x < Dimensions.x; x++)
        {
            for(int y = 0; y < Dimensions.y; y++)
            {
                Vector2Int v = new Vector2Int(x, y);
                if (!UnoccupiedTiles.Contains(v)) tiles.Add(v);
            }
        }

        // Add interaction spots
        tiles.AddRange(InteractionSpotTiles);

        return tiles;
    }


    private List<OperatingMode> _OperatingModes;
    protected abstract List<OperatingMode> OperatingModes { get; }
    public List<OperatingMode> GetOperatingModes() => _OperatingModes;

    public override bool Validate()
    {
        _OperatingModes = OperatingModes;

        if (InteractionSpotTiles.Count < _OperatingModes.Max(o => o.NumCrewMembers)) throw new System.Exception("Not enough interaction spots for all operating modes.");

        return base.Validate();
    }
}

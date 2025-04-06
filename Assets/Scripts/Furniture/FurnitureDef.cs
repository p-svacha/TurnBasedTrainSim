using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FurnitureDef : Def
{
    /// <summary>
    /// The path after Resources/ leading to the prefab that gets instantiated to represent this furniture.
    /// </summary>
    public abstract string PrefabResourcePath { get; }

    /// <summary>
    /// The size (amount of tiles) this furniture occupies when rotated default (North).
    /// <br/>By default all tiles within the dimensions will count as blocked.
    /// <br/>Individual tiles can be set as empty or interaction spot tiles with separate lists.
    /// </summary>
    public abstract Vector2Int Dimensions { get; }
    public virtual List<Vector2Int> UnoccupiedTiles { get; } = new List<Vector2Int>();
    public virtual List<Vector2Int> InteractionSpotTiles { get; } = new List<Vector2Int>();

    public List<OperatingMode> OperatingModes { get; private set; }
    protected abstract List<OperatingMode> OperatingModeDefs { get; }

    public override bool Validate()
    {
        OperatingModes = OperatingModeDefs;

        // todo: validate operating modes (i.e. enough interaction spots for crew requirement)

        return base.Validate();
    }
}

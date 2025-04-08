using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileOccupation
{
    /// <summary>
    /// This tile is empty. Furniture can be placed on it and crew can walk over it.
    /// </summary>
    Empty = 0,

    /// <summary>
    /// This tile is blocked by something. Nothing can be placed here and noone can walk here.
    /// </summary>
    Blocked = 1,

    /// <summary>
    /// This tile is an interaction spot of a furniture. No other furniture can be placed here, but crew can walk over it (even if someone is working here).
    /// </summary>
    InteractionSpot = 2,
}

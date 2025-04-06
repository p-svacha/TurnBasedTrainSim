using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Each furniture has one operating mode active at a time. A mode can have requirements, effects and modifiers on both.
/// </summary>
public class OperatingMode
{
    /// <summary>
    /// The display name of the mode.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// A short flavoured description of the mode.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// How many crew members are required for this mode.
    /// </summary>
    public int NumCrewMembers { get; init; } = 0;

    /// <summary>
    /// The resources this mode gives.
    /// </summary>
    public Dictionary<ResourceDef, int> OutputResources;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WagonPartDef : Def
{
    /// <summary>
    /// The path after Resources/ leading to the prefab that gets instantiated to represent this furniture.
    /// </summary>
    public abstract string PrefabResourcePath { get; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WagonLayoutDef : Def
{
    /// <summary>
    /// The amount of tiles the wagon has in the x axis (forwards-backwards direction)
    /// </summary>
    public abstract int Length { get; }

    /// <summary>
    /// The amount of tiles the wagon has in the y axis (sideways direction)
    /// </summary>
    public abstract int Width { get; }
}

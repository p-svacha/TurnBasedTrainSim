using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The walls and roof of a wagon.
/// </summary>
public class Frame : WagonPart
{
    public new FrameDef Def => (FrameDef)base.Def;
}

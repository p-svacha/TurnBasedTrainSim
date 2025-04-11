using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A set of 2 wheels (left + right).
/// </summary>
public class Wheels : WagonPart
{
    public new WheelsDef Def => (WheelsDef)base.Def;
}

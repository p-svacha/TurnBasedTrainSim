using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonLayout_Short : WagonLayoutDef
{
    public override int Length => 12;
    public override int Width => 6;
    public override string DefName => "Short";
    public override string Label => $"short ({Length}x{Width})";
    public override string Description => throw new System.NotImplementedException();
}

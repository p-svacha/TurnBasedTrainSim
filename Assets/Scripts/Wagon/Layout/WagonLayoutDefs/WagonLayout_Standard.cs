using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonLayout_Standard : WagonLayoutDef
{
    public override int Length => 24;
    public override int Width => 6;
    public override string DefName => "Standard";
    public override string Label => $"standard ({Length}x{Width})";
    public override string Description => throw new System.NotImplementedException();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonLayout_Long : WagonLayoutDef
{
    public override int Length => 36;
    public override int Width => 6;
    public override string DefName => "Long";
    public override string Label => $"long ({Length}x{Width})";
    public override string Description => throw new System.NotImplementedException();
}

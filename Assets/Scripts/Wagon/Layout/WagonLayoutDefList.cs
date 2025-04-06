using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WagonLayoutDefList
{
    public static List<WagonLayoutDef> Defs = new List<WagonLayoutDef>()
    {
        new WagonLayout_Short(),
        new WagonLayout_Standard(),
        new WagonLayout_Long(),
    };
}

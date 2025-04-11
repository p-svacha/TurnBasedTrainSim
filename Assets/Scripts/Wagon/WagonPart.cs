using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WagonPart : MonoBehaviour
{
    public WagonPartDef Def { get; private set; }

    public void Init(WagonPartDef def)
    {
        Def = def;
    }

    public int Weight => Def.Weight;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpokedWheels : WheelsDef
{
    public override string DefName => "WoodenSpokedWheels";
    public override string Label => "Wooden Spoked Wheels";
    public override string Description => "Simple, handcrafted wheels made from hardwood and iron bands. Reliable on flat, well-maintained tracks, but prone to wear and instability at higher speeds or under heavy loads. The bare minimum for mobility.";

    public override string PrefabResourcePath => "Prefabs/WagonParts/Wheels/DefaultWheels";

    public override int Weight => 60;
}

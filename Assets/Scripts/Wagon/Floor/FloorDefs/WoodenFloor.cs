using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenFloor : FloorDef
{
    public override string DefName => "WoodenFloor";
    public override string Label => "Wooden Floor";
    public override string Description => "A basic wooden platform forming the foundation of a train wagon. Built from heavy planks and barely reinforced, it’s sturdy enough to carry light cargo or crew, but offers little insulation or protection. A humble start for a long journey.";

    public override string PrefabResourcePath => "Prefabs/WagonParts/Floors/WoodenFloor";

    public override int Weight => 450;
}

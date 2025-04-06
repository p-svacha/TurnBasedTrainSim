using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCarEngineDef : FurnitureDef
{
    public override string DefName => "HandcarEngine";
    public override string Label => "handcar engine";
    public override string Description => "A simple, manually-powered platform that moves the train using crew effort. Requires 1-2 crew members to operate and generates up to 3 PP.";
    public override string PrefabResourcePath => "Prefabs/Furniture/Engines/HandcarEngine";

    public override Vector2Int Dimensions => new Vector2Int(6, 4);
    // todo: interaction spots

    protected override List<OperatingMode> OperatingModeDefs => new List<OperatingMode>()
    {
        new OperatingMode()
        {
            Name = "Inactive"
        },
        new OperatingMode()
        {
            Name = "One-Person Operation",
            Description = "One crew member pumps the engine for some minor propulsion.",
            NumCrewMembers = 1,
            OutputResources = new Dictionary<ResourceDef, int>()
            {
                { ResourceDefOf.PropulsionPower, 1 }
            }
        },
        new OperatingMode()
        {
            Name = "Two-Person Operation",
            Description = "Two crew members pump the engine for full output.",
            NumCrewMembers = 2,
            OutputResources = new Dictionary<ResourceDef, int>()
            {
                { ResourceDefOf.PropulsionPower, 3 }
            }
        },
    };
}

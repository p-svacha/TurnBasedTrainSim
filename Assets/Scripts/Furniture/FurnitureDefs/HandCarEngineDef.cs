using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCarEngineDef : FurnitureDef
{
    public override string DefName => "HandcarEngine";
    public override string Label => "handcar engine";
    public override string Description => "A simple, manually-powered platform that moves the train using crew effort. Requires 1-2 crew members to operate and generates up to 3 PP.";
    public override string PrefabResourcePath => "Prefabs/Furniture/Engines/HandcarEngine";

    public override Vector2Int Dimensions => new Vector2Int(4, 4);
    public override List<InteractionSpot> InteractionSpots => new List<InteractionSpot>()
    {
        new InteractionSpot(-1, 1, Direction.E),
        new InteractionSpot(4, 2, Direction.W),
    };

    protected override List<OperatingMode> OperatingModes => new List<OperatingMode>()
    {
        new OperatingMode()
        {
            Label = "Inactive"
        },
        new OperatingMode()
        {
            Label = "One-Person Operation",
            AssignLabel = "Operate",
            Description = "One crew member pumps the engine for some minor propulsion.",
            NumCrewMembers = 1,
            OutputResources = new Dictionary<ResourceDef, int>()
            {
                { ResourceDefOf.PropulsionPower, 1 }
            }
        },
        new OperatingMode()
        {
            Label = "Two-Person Operation",
            AssignLabel = "Operate",
            Description = "Two crew members pump the engine for full output.",
            NumCrewMembers = 2,
            OutputResources = new Dictionary<ResourceDef, int>()
            {
                { ResourceDefOf.PropulsionPower, 3 }
            }
        },
    };
}

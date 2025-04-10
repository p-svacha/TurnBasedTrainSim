using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public FurnitureDef Def;

    public Game Game { get; private set; }

    public Tile Origin { get; private set; }
    public Wagon Wagon => Origin.Wagon;
    public Direction Rotation { get; private set; }
    public bool IsMirrored { get; private set; }

    /// <summary>
    /// The operating mode currently active on this furniture.
    /// </summary>
    public OperatingMode CurrentOperatingMode { get; private set; }

    public List<Character> AssignedCharacters { get; private set; }


    public void Init(Game game, FurnitureDef def, Tile origin, Direction rotation, bool isMirrored)
    {
        Game = game;

        Def = def;
        Origin = origin;
        Rotation = rotation;
        IsMirrored = isMirrored;

        CurrentOperatingMode = def.GetOperatingModes()[0];

        AssignedCharacters = new List<Character>();
    }

    public void SetAssignedCharacters(List<Character> characters)
    {
        // Set assigned characters
        AssignedCharacters = characters;

        // Move characters to interactions spots
        List<InteractionSpot> interactionSpots = GetInteractionSpots();
        for(int i = 0; i < characters.Count; i++)
        {
            characters[i].TeleportOnTile(interactionSpots[i].GetWagonTile(Wagon), interactionSpots[i].Direction);
        }
    }

    



    #region Getters

    /// <summary>
    /// Returns a list of all actions that can be performed by the player when rightclicking this furniture.
    /// <br/>Actions may vary depending on what is currently selected.
    /// </summary>
    public List<ContextMenuOption> GetActions()
    {
        List<ContextMenuOption> options = new List<ContextMenuOption>();

        if (Game.SelectedCharacter != null)
        {
            // Get operation modes that could get active when character gets assigned
            List<Character> newAssignedCharacters = new List<Character>(AssignedCharacters);
            newAssignedCharacters.Add(Game.SelectedCharacter);
            List<OperatingMode> assignModes = Def.GetOperatingModes().Where(mode => mode.CanBeActive(newAssignedCharacters)).ToList();

            foreach (OperatingMode mode in assignModes) options.Add(new ContextMenuOption(mode.AssignLabel, () => Game.SetOperatingMode(this, mode, newAssignedCharacters)));
        }

        return options;
    }

    /// <summary>
    /// Returns all tiles that are completely blocked by this furniture.
    /// </summary>
    public List<Tile> GetBlockedTiles()
    {
        List<Tile> blockedTiles = new List<Tile>();
        for (int localX = 0; localX < Def.Dimensions.x; localX++)
        {
            for (int localY = 0; localY < Def.Dimensions.y; localY++)
            {
                Vector2Int localPos = new Vector2Int(localX, localY);
                if (!Def.UnoccupiedTiles.Contains(localPos))
                {
                    Vector2Int translatedPos = HelperFunctions.GetTranslatedPosition(localPos, Def.Dimensions, Rotation, IsMirrored);
                    Vector2Int finalTilePos = Origin.Coordinates + translatedPos;
                    blockedTiles.Add(Wagon.GetTile(finalTilePos));
                }
            }
        }
        return blockedTiles;
    }

    /// <summary>
    /// Returns the positions (wagon coordinates) and rotations of all interaction spots of this furniture, ordered by usage priority.
    /// </summary>
    public List<InteractionSpot> GetInteractionSpots()
    {
        List<InteractionSpot> interactionSpots = new List<InteractionSpot>();
        foreach (InteractionSpot spot in Def.InteractionSpots)
        {
            Vector2Int translatedPos = HelperFunctions.GetTranslatedPosition(spot.Position, Def.Dimensions, Rotation, IsMirrored);
            Vector2Int finalPos = Origin.Coordinates + translatedPos;
            if (finalPos.x < 0 || finalPos.x >= Wagon.Length || finalPos.y < 0 || finalPos.y >= Wagon.Width) throw new System.Exception($"Interaction spot outside of wagon! {finalPos}");
            Direction finalDir = HelperFunctions.GetRotatedDirection(spot.Direction, Rotation);

            interactionSpots.Add(new InteractionSpot(finalPos, finalDir));
        }

        return interactionSpots;
    }

    public string LabelCap => Def.LabelCap;

    #endregion

}

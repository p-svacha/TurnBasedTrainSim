using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler
{
    public Game Game;

    public InputHandler(Game game)
    {
        Game = game;
    }

    public void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.O)) Game.ToggleTileOccupationOverlay();

        if (Input.GetMouseButtonDown(0)) OnLeftClick();
        else if (Input.GetMouseButtonDown(1)) OnRightClick();
    }

    public void OnLeftClick()
    {
        if (WorldManager.HoveredCharacter != null)
        {
            OnCharacterLeftClicked(WorldManager.HoveredCharacter);
        }
        else if (WorldManager.HoveredFurniture != null)
        {
            OnFurnitureLeftClicked(WorldManager.HoveredFurniture);
        }
        else OnNothingLeftClicked();
    }

    public void OnRightClick()
    {
        if (WorldManager.HoveredCharacter != null)
        {
            OnCharacterRightClicked(WorldManager.HoveredCharacter);
        }
        else if (WorldManager.HoveredFurniture != null)
        {
            OnFurnitureRightClicked(WorldManager.HoveredFurniture);
        }
        else OnNothingRightClicked();
    }

    public void OnCharacterLeftClicked(Character c)
    {
        if (c == Game.SelectedCharacter) Game.DeselectCharacter();
        else Game.SelectCharacter(c);
    }
    public void OnCharacterRightClicked(Character c)
    {

    }

    public void OnFurnitureLeftClicked(Furniture f)
    {
        Debug.Log("FLC");
    }
    public void OnFurnitureRightClicked(Furniture f)
    {

    }

    public void OnNothingLeftClicked()
    {
        Game.DeselectCharacter();
    }
    public void OnNothingRightClicked()
    {
    }
}

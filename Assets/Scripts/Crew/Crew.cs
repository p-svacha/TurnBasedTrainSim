using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crew : MonoBehaviour
{
    public Game Game;
    public Train Train => Game.Train;
    public List<Character> Characters = new List<Character>();

    public void Init(Game game)
    {
        Game = game;
    }

    public void CreateStarterCrew()
    {
        for(int i = 0; i < 3; i++)
        {
            AddCharacterToCrew();
        }
    }

    public void AddCharacterToCrew()
    {
        GameObject characterObject = Instantiate(ResourceManager.LoadPrefab("Prefabs/Characters/Human"), transform);
        characterObject.layer = WorldManager.Layer_Crew;
        Character character = characterObject.AddComponent<Character>();
        character.TeleportOnTile(Train.GetRandomEmptyTile(), HelperFunctions.GetRandomDirection4());
        Characters.Add(character);
    }
}

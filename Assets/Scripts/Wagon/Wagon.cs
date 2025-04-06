using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    private const float WHEELS_X_OFFSET = 4f;

    /// <summary>
    /// Amount of tiles in x axis (forward-direction)
    /// </summary>
    public int Length = 24;

    /// <summary>
    /// Amount of tiles in y axis (sideways-direction)
    /// </summary>
    public int Width = 6;

    public Tile[,] Tiles;

    public Wheels FrontWheels;
    public Wheels BackWheels;
    public Floor Floor;
    public Frame Frame;

    public void InitializeStarterWagon()
    {
        GameObject frontWheelObject = Instantiate(ResourceManager.LoadPrefab("Prefabs/WagonParts/Wheels/DefaultWheels"), transform);
        SetLayer(frontWheelObject, WorldManager.Layer_Wagon);
        FrontWheels = frontWheelObject.AddComponent<Wheels>();
        FrontWheels.transform.localPosition = new Vector3(WHEELS_X_OFFSET, 0f, 0f);

        GameObject backWheelObject = Instantiate(ResourceManager.LoadPrefab("Prefabs/WagonParts/Wheels/DefaultWheels"), transform);
        SetLayer(backWheelObject, WorldManager.Layer_Wagon);
        BackWheels = backWheelObject.AddComponent<Wheels>();
        BackWheels.transform.localPosition = new Vector3(-WHEELS_X_OFFSET, 0f, 0f);

        GameObject floorObject = Instantiate(ResourceManager.LoadPrefab("Prefabs/WagonParts/Floors/DefaultFloor"), transform);
        SetLayer(floorObject, WorldManager.Layer_Wagon);
        Floor = floorObject.AddComponent<Floor>();
        Floor.transform.localPosition = new Vector3(0f, -0.25f, 0f);

        InitializeTiles();
    }

    /// <summary>
    /// Sets the layer of a GameObject and all its children.
    /// </summary>
    private void SetLayer(GameObject obj, int layer)
    {
        obj.layer = WorldManager.Layer_Wagon;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.layer = layer;
        }
    }

    private void InitializeTiles()
    {
        Tiles = new Tile[Length, Width];
        for(int x = 0; x < Length; x++)
        {
            for(int y = 0; y < Width; y++)
            {
                Tiles[x, y] = new Tile(this, new Vector2Int(x, y));
            }
        }
    }

    public Tile GetRandomTile()
    {
        return Tiles[Random.Range(0, Length), Random.Range(0, Width)];
    }
}

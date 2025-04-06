using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    private const float WHEELS_X_OFFSET = 4f;

    public Tile[,] Tiles;

    public WagonLayoutDef Layout;
    public Wheels FrontWheels;
    public Wheels BackWheels;
    public Floor Floor;
    public Frame Frame;

    public List<Furniture> Furniture = new List<Furniture>();

    public Furniture AddNewFurniture(FurnitureDef furnitureDef, Vector2Int position, Direction rotation)
    {
        Furniture newFurniture = FurnitureCreator.SpawnFurniture(furnitureDef, this, Tiles[position.x, position.y], rotation);
        Furniture.Add(newFurniture);
        return newFurniture;
    }

    public void Initialize(WagonLayoutDef layout, Wheels frontWheels, Wheels backWheels, Floor floor, Frame frame)
    {
        // Layout
        Layout = layout;

        // Tiles
        Tiles = new Tile[Length, Width];
        for(int x = 0; x < Length; x++)
        {
            for(int y = 0; y < Width; y++)
            {
                Tiles[x, y] = new Tile(this, new Vector2Int(x, y));
            }
        }

        // Parts
        FrontWheels = frontWheels;
        BackWheels = backWheels;
        Floor = floor;
        Frame = frame;
    }

    public Tile GetRandomTile()
    {
        return Tiles[Random.Range(0, Length), Random.Range(0, Width)];
    }

    #region Getters

    /// <summary>
    /// Amount of tiles in x axis (forward-direction)
    /// </summary>
    public int Length => Layout.Length;

    /// <summary>
    /// Amount of tiles in y axis (sideways-direction)
    /// </summary>
    public int Width => Layout.Width;

    #endregion
}

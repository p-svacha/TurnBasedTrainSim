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

    public Furniture AddNewFurniture(FurnitureDef furnitureDef, Vector2Int position, Direction rotation, bool isMirrored)
    {
        Furniture newFurniture = FurnitureCreator.CreateFurniture(furnitureDef, Tiles[position.x, position.y], rotation, isMirrored);
        Furniture.Add(newFurniture);
        UpdateTileOccupation();
        return newFurniture;
    }

    private void UpdateTileOccupation()
    {
        // Empty every tile
        for (int x = 0; x < Length; x++)
        {
            for (int y = 0; y < Width; y++)
            {
                Tiles[x, y].Occupation = TileOccupation.Empty;
            }
        }

        // Iterate through each furniture to set blocked and interaction spot tiles
        foreach(Furniture furniture in Furniture)
        {
            // Blocked tiles
            for (int localX = 0; localX < furniture.Def.Dimensions.x; localX++)
            {
                for (int localY = 0; localY < furniture.Def.Dimensions.y; localY++)
                {
                    Vector2Int localPos = new Vector2Int(localX, localY);
                    if (!furniture.Def.UnoccupiedTiles.Contains(localPos))
                    {
                        Vector2Int translatedPos = HelperFunctions.GetTranslatedPosition(localPos, furniture.Def.Dimensions, furniture.Rotation, furniture.IsMirrored);
                        Vector2Int finalTilePos = furniture.Origin.Coordinates + translatedPos;
                        Debug.Log($"final pos is {finalTilePos}");
                        Tiles[finalTilePos.x, finalTilePos.y].Occupation = TileOccupation.Blocked;
                    }
                }
            }

            // Interaction spot tiles
            foreach(Vector2Int interactionSpotPos in furniture.Def.InteractionSpotTiles)
            {
                Vector2Int translatedPos = HelperFunctions.GetTranslatedPosition(interactionSpotPos, furniture.Def.Dimensions, furniture.Rotation, furniture.IsMirrored);
                Vector2Int finalTilePos = furniture.Origin.Coordinates + translatedPos;
                if (finalTilePos.x < 0 || finalTilePos.x >= Length || finalTilePos.y < 0 || finalTilePos.y >= Width) throw new System.Exception($"Interaction spot outside of wagon! {finalTilePos}");
                Tiles[finalTilePos.x, finalTilePos.y].Occupation = TileOccupation.InteractionSpot;
            }
        }

        // Pass new values to floor shader
        float[] shaderValues = new float[256];
        int index = 0;
        for (int x = 0; x < Length; x++)
        {
            for (int y = 0; y < Width; y++)
            {
                shaderValues[index] = (int)Tiles[x, y].Occupation;
                Debug.Log(shaderValues[index]);
                index++;
            }
        }
        Floor.GetComponent<MeshRenderer>().material.SetFloatArray("_TileOccupation", shaderValues);
    }

    public void ShowTileOccupationOverlay(bool value)
    {
        Floor.GetComponent<MeshRenderer>().material.SetFloat("_ShowTileOccupation", value ? 1 : 0);
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

    public Tile GetRandomTile()
    {
        return Tiles[Random.Range(0, Length), Random.Range(0, Width)];
    }

    #endregion
}

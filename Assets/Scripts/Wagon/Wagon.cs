using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    private const float WHEELS_X_OFFSET = 4f;

    public Train Train;
    public Game Game => Train.Game;

    private Tile[,] Tiles;

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

    /// <summary>
    /// Recalculates the occupation of each tile in this wagon.
    /// </summary>
    public void UpdateTileOccupation()
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
            foreach (Tile blockedTile in furniture.GetBlockedTiles())
            {
                blockedTile.Occupation = TileOccupation.Blocked;
            }

            // Interaction spot tiles
            foreach(InteractionSpot interactionSpot in furniture.GetInteractionSpots())
            {
                interactionSpot.GetWagonTile(this).Occupation = TileOccupation.InteractionSpot;
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

    public Tile GetTile(Vector2Int pos) => GetTile(pos.x, pos.y);
    public Tile GetTile(int x, int y)
    {
        return Tiles[x, y];
    }

    public Tile GetRandomTile()
    {
        return Tiles[Random.Range(0, Length), Random.Range(0, Width)];
    }

    public Tile GetRandomEmptyTile()
    {
        return Tiles.Cast<Tile>().Where(t => t.Occupation == TileOccupation.Empty).ToList().RandomElement();
    }

    #endregion
}

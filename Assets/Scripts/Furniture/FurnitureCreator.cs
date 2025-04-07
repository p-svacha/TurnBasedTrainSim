using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FurnitureCreator
{
    /// <summary>
    /// Creates a furniture somewhere in the train.
    /// </summary>
    /// <param name="def">The Def of the furniture that should be created.</param>
    /// <param name="tile">The exact tile the furniture should be created on. This is the south-western most tile of the furniture.</param>
    /// <param name="rotation">The rotation of the furniture. Direction.N means the dimensions and position will be exactly like in the def. In other rotations they will be translated accordingly.</param>
    public static Furniture CreateFurniture(FurnitureDef def, Tile tile, Direction rotation)
    {
        if (def == null) throw new System.Exception("The provided FurnitureDef is null.");

        GameObject prefab = ResourceManager.LoadPrefab(def.PrefabResourcePath);
        GameObject furnitureObject = GameObject.Instantiate(prefab);
        Furniture furniture = furnitureObject.AddComponent<Furniture>();

        Vector3 tileCenter = tile.GetWorldPosition();
        float posX = tileCenter.x + (def.Dimensions.x * 0.5f * Tile.TILE_SIZE) - (Tile.TILE_SIZE * 0.5f);
        float posZ = tileCenter.z + (def.Dimensions.y * 0.5f * Tile.TILE_SIZE) - (Tile.TILE_SIZE * 0.5f);
        furniture.transform.position = new Vector3(posX, tileCenter.y, posZ);

        return furniture;
    }
}

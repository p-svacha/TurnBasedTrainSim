using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FurnitureCreator
{
    /// <summary>
    /// Creates a furniture somewhere in the train.
    /// </summary>
    /// <param name="furniture">The Def of the furniture that should be created.</param>
    /// <param name="tile">The exact tile the furniture should be created on. This is the south-western most tile of the furniture.</param>
    /// <param name="rotation">The rotation of the furniture. Direction.N means the dimensions and position will be exactly like in the def. In other rotations they will be translated accordingly.</param>
    public static Furniture SpawnFurniture(FurnitureDef def, Wagon wagon, Tile tile, Direction rotation)
    {
        if (def == null) throw new System.Exception("The provided FurnitureDef is null.");

        GameObject prefab = ResourceManager.LoadPrefab(def.PrefabResourcePath);
        GameObject furnitureObject = GameObject.Instantiate(prefab);
        Furniture furniture = furnitureObject.AddComponent<Furniture>();

        return furniture;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WagonManager
{
    public static Wagon CreateWagon(WagonLayoutDef layoutDef, WheelsDef frontWheelsDef, WheelsDef backWheelsDef, FloorDef floorDef, FrameDef frameDef)
    {
        GameObject wagonObject = new GameObject("Wagon");
        Wagon wagon = wagonObject.AddComponent<Wagon>();

        GameObject frontWheelObject = GameObject.Instantiate(ResourceManager.LoadPrefab(frontWheelsDef.PrefabResourcePath), wagon.transform);
        HelperFunctions.SetLayer(frontWheelObject, WorldManager.Layer_Wagon);
        Wheels frontWheels = frontWheelObject.AddComponent<Wheels>();
        frontWheels.transform.localPosition = new Vector3(layoutDef.Length * 0.15f, 0f, 0f);

        GameObject backWheelObject = GameObject.Instantiate(ResourceManager.LoadPrefab(backWheelsDef.PrefabResourcePath), wagon.transform);
        HelperFunctions.SetLayer(backWheelObject, WorldManager.Layer_Wagon);
        Wheels backWheels = backWheelObject.AddComponent<Wheels>();
        backWheels.transform.localPosition = new Vector3(-layoutDef.Length * 0.15f, 0f, 0f);

        string floorPrefabPath = floorDef.PrefabResourcePath + "_" + layoutDef.DefName;
        GameObject floorObject = GameObject.Instantiate(ResourceManager.LoadPrefab(floorPrefabPath), wagon.transform);
        floorObject.GetComponent<MeshRenderer>().material.SetFloat("_WagonLength", layoutDef.Length);
        floorObject.GetComponent<MeshRenderer>().material.SetFloat("_WagonWidth", layoutDef.Width);
        HelperFunctions.SetLayer(floorObject, WorldManager.Layer_Wagon);
        Floor floor = floorObject.AddComponent<Floor>();
        floor.transform.localPosition = new Vector3(0f, -0.25f, 0f);

        Frame frame = null;
        if(frameDef != null)
        {
            GameObject frameObject = GameObject.Instantiate(ResourceManager.LoadPrefab(frameDef.PrefabResourcePath), wagon.transform);
            HelperFunctions.SetLayer(frameObject, WorldManager.Layer_Wagon);
            frame = floorObject.AddComponent<Frame>();
        }

        wagon.Initialize(layoutDef, frontWheels, backWheels, floor, frame);

        return wagon;
    }
}

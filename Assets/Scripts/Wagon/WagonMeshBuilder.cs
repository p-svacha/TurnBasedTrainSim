using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WagonMeshBuilder
{
    public static Wagon BuildWagon()
    {
        GameObject wagonObject = new GameObject("wagon");

        Wagon wagon = wagonObject.AddComponent<Wagon>();
        return wagon;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public FurnitureDef Def;

    public Tile Origin;
    public Direction Rotation;
    public bool IsMirrored;
    

    public void Init(FurnitureDef def, Tile origin, Direction rotation, bool isMirrored)
    {
        Def = def;
        Origin = origin;
        Rotation = rotation;
        IsMirrored = isMirrored;
    }

}

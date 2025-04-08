using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    /// <summary>
    /// Sets the layer of a GameObject and all its children.
    /// </summary>
    public static void SetLayer(GameObject obj, int layer)
    {
        obj.layer = WorldManager.Layer_Wagon;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            obj.transform.GetChild(i).gameObject.layer = layer;
        }
    }

    /// <summary>
    /// Returns the translated dimensions given the rotation.
    /// </summary>
    public static Vector2Int GetTranslatedDimensions2d(Vector2Int sourceDimensions, Direction rotation)
    {
        if (rotation == Direction.N || rotation == Direction.S) return sourceDimensions;
        if (rotation == Direction.E || rotation == Direction.W) return new Vector2Int(sourceDimensions.y, sourceDimensions.x);
        throw new System.Exception(rotation.ToString() + " is not a valid rotation");
    }

    /// <summary>
    /// Returns the translated position with the specified dimensions, given its rotation (default = N) and if it is mirrored (on the x axis).
    /// </summary>
    public static Vector2Int GetTranslatedPosition(Vector2Int position, Vector2Int dimensions, Direction rotation, bool isMirrored)
    {
        Vector2Int translatedDimensions = GetTranslatedDimensions2d(dimensions, rotation);

        int x = position.x;
        int y = position.y;

        int rotatedX, rotatedY;
        switch (rotation)
        {
            case Direction.N:
                rotatedX = x;
                rotatedY = y;
                break;

            case Direction.W:
                rotatedX = y;
                rotatedY = translatedDimensions.x - 1 - x;
                break;

            case Direction.S:
                rotatedX = translatedDimensions.x - 1 - x;
                rotatedY = translatedDimensions.y - 1 - y;
                break;

            case Direction.E:
                rotatedX = translatedDimensions.y - 1 - y;
                rotatedY = x;
                break;

            default:
                throw new System.Exception("Invalid rotation");
        }

        if (isMirrored)
        {
            rotatedX = dimensions.x - 1 - rotatedX;
        }

        // Return the final transformed coordinate
        return new Vector2Int(rotatedX, rotatedY);
    }

    public static float GetRotationAngle(Direction dir)
    {
        if (dir == Direction.N) return 0f;
        if (dir == Direction.E) return 90;
        if (dir == Direction.S) return 180f;
        if (dir == Direction.W) return 270f;
        return 0f;
    }

    public static Quaternion GetRotation(Direction dir)
    {
        return Quaternion.Euler(0f, GetRotationAngle(dir), 0f);
    }

}

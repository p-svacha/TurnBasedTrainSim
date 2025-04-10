using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class HelperFunctions
{

    #region GameObject

    /// <summary>
    /// Sets the layer of a GameObject and all its children.
    /// </summary>
    public static void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SetLayer(obj.transform.GetChild(i).gameObject, layer);
        }
    }

    /// <summary>
    /// Searches for a component of type T on the provided GameObject and all its parent objects until one is found.
    /// </summary>
    /// <typeparam name="T">The type of component to search for.</typeparam>
    /// <param name="gameObject">The starting GameObject to search on.</param>
    /// <returns>The component of type T if found; otherwise, returns null.</returns>
    public static T GetComponentInParentChain<T>(GameObject gameObject) where T : Component
    {
        if (gameObject == null)
        {
            return null;
        }

        // Check if the component exists on the current GameObject.
        T component = gameObject.GetComponent<T>();
        if (component != null)
        {
            return component;
        }

        // Traverse the parent hierarchy until the component is found.
        Transform parentTransform = gameObject.transform.parent;
        while (parentTransform != null)
        {
            component = parentTransform.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
            parentTransform = parentTransform.parent;
        }

        // Return null if the component was not found on any parent.
        return null;
    }

    #endregion

    #region Direction

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

    public static int GetRotationAngle(Direction dir)
    {
        if (dir == Direction.N) return 0;
        if (dir == Direction.E) return 90;
        if (dir == Direction.S) return 180;
        if (dir == Direction.W) return 270;
        return 0;
    }

    public static Direction GetDirectionFromAngle(int angle)
    {
        angle = angle % 360;
        if (angle == 0) return Direction.N;
        if (angle == 90) return Direction.E;
        if (angle == 180) return Direction.S;
        if (angle == 270) return Direction.W;
        throw new System.Exception($"Angle {angle} does not result in a clear direction.");
    }

    public static Direction GetRotatedDirection(Direction sourceDir, Direction rotation)
    {
        int initAngle = GetRotationAngle(sourceDir);
        int rotationAngle = GetRotationAngle(rotation);
        int finalAngle = initAngle + rotationAngle;
        return GetDirectionFromAngle(finalAngle);
    }

    public static Quaternion GetRotation(Direction dir, int offset = 0)
    {
        return Quaternion.Euler(0f, GetRotationAngle(dir) + offset, 0f);
    }

    public static Direction GetRandomDirection4()
    {
        int rng = Random.Range(0, 4);
        if (rng == 0) return Direction.N;
        if (rng == 1) return Direction.E;
        if (rng == 2) return Direction.S;
        if (rng == 3) return Direction.W;
        throw new System.Exception("Error");
    }

    #endregion

    #region UI

    /// <summary>
    /// Destroys all children of a GameObject immediately.
    /// </summary>
    public static void DestroyAllChildredImmediately(GameObject obj, int skipElements = 0)
    {
        int numChildren = obj.transform.childCount;
        for (int i = skipElements; i < numChildren; i++) GameObject.DestroyImmediate(obj.transform.GetChild(skipElements).gameObject);
    }

    public static Sprite TextureToSprite(Texture tex) => TextureToSprite((Texture2D)tex);
    public static Sprite TextureToSprite(Texture2D tex)
    {
        if (tex == null) return null;
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
    public static Sprite GetAssetPreviewSprite(string path)
    {
#if UNITY_EDITOR
        // Only executes in the Unity Editor
        UnityEngine.Object asset = Resources.Load(path);
        if (asset == null)
            throw new System.Exception($"Could not find asset with path {path}.");

        // The AssetPreview class is also editor-only
        Texture2D assetPreviewTexture = UnityEditor.AssetPreview.GetAssetPreview(asset);
        // if (assetPreviewTexture == null) 
        //    throw new System.Exception($"Could not create asset preview texture of {asset} ({path}).");

        return TextureToSprite(assetPreviewTexture);
#else
    // Always returns null in builds
    return null;
#endif
    }

    public static Sprite TextureToSprite(string resourcePath)
    {
        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        return TextureToSprite(texture);
    }

    /// <summary>
    /// Sets the Left, Right, Top and Bottom attribute of a RectTransform
    /// </summary>
    public static void SetRectTransformMargins(RectTransform rt, float left, float right, float top, float bottom)
    {
        rt.offsetMin = new Vector2(left, bottom);
        rt.offsetMax = new Vector2(-right, -top);
    }

    public static void SetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    /// <summary>
    /// Unfocusses any focussed button/dropdown/toggle UI element so that keyboard inputs don't get 'absorbed' by the UI element.
    /// </summary>
    public static void UnfocusNonInputUiElements()
    {
        if (EventSystem.current.currentSelectedGameObject != null && (
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null ||
            EventSystem.current.currentSelectedGameObject.GetComponent<TMP_Dropdown>() != null ||
            EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>() != null
            ))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// Returns if any ui element is currently focussed.
    /// </summary>
    public static bool IsUiFocussed()
    {
        return EventSystem.current.currentSelectedGameObject != null;
    }

    /// <summary>
    /// Returns is the mouse is currently hovering over a UI element.
    /// </summary>
    public static bool IsMouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static bool IsPointerOverUIWithComponent<T>() where T : Component
    {
        EventSystem eventSystem = EventSystem.current;
        GraphicRaycaster raycaster = Object.FindFirstObjectByType<GraphicRaycaster>();

        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<T>() != null)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the mouse is currently over a UI element, excluding certain UI objects
    /// and all their children.
    /// </summary>
    /// <param name="excludedUiElements">
    /// Optional list of UI GameObjects to ignore in the check (including any of their children).
    /// </param>
    /// <returns>
    /// True if mouse is over a UI element that is not excluded; false otherwise.
    /// </returns>
    public static bool IsMouseOverUiExcept(params GameObject[] excludedUiElements)
    {
        // Quick check: if pointer isn't over *any* UI elements, we can stop.
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        // Perform a UI raycast from the mouse pointer
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // If no UI elements are hit, we can stop
        if (results.Count == 0)
        {
            return false;
        }

        // Check each UI element that was hit by the raycast
        foreach (RaycastResult result in results)
        {
            GameObject hitObject = result.gameObject;

            // If the hit object is not in the excluded list and not a child of an excluded object,
            // then we consider the mouse to be over a "meaningful" UI element.
            if (!IsExcluded(hitObject, excludedUiElements))
            {
                return true;
            }
        }

        // If we only hit excluded objects, return false
        return false;
    }

    /// <summary>
    /// Returns true if the given object is the same as one of the excluded objects
    /// or is a child of one of them.
    /// </summary>
    private static bool IsExcluded(GameObject candidate, GameObject[] excludedUiElements)
    {
        foreach (GameObject excluded in excludedUiElements)
        {
            if (excluded == null) continue;

            // If candidate is the excluded object itself or is a descendant
            if (candidate.transform == excluded.transform ||
                candidate.transform.IsChildOf(excluded.transform))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for identifying hovered objects.
/// </summary>
public static class WorldManager
{
    // Layers
    public static int Layer_Wagon;
    public static int Layer_Crew;
    public static int Layer_Furniture;

    // Hovered Objects
    public static GameObject HoveredObject { get; private set; }
    public static Character HoveredCharater { get; private set; }
    public static Furniture HoveredFurniture { get; private set; }

    public static void Initialize()
    {
        Layer_Wagon = LayerMask.NameToLayer("Wagon");
        Layer_Furniture = LayerMask.NameToLayer("Furniture");
        Layer_Crew = LayerMask.NameToLayer("Crew");
    }

    public static void UpdateHoveredObjects()
    {
        GameObject newHoveredObject = null;
        Character newHoveredCharacter = null;
        Furniture newHoveredFurniture = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            newHoveredObject = hit.collider.gameObject;
            Debug.Log("Pointing at: " + newHoveredObject.name);

            if(newHoveredObject.layer == Layer_Crew)
            {
                newHoveredCharacter = newHoveredObject.GetComponent<Character>();
            }
            if (newHoveredObject.layer == Layer_Furniture)
            {
                newHoveredFurniture = newHoveredObject.GetComponent<Furniture>();
            }
        }

        HoveredObject = newHoveredObject;
        HoveredFurniture = newHoveredFurniture;
        HoveredCharater = newHoveredCharacter;
    }
}

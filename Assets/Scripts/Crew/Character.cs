using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Simulation
    public Job Job;

    // Visual
    public Tile CurrentTile;
    private GameObject SelectionIndicator;
    private const float SELECTION_INDICATOR_Y_OFFSET = 1.6f;

    public void TeleportOnTile(Tile tile)
    {
        CurrentTile = tile;
        transform.position = tile.GetWorldPosition();
    }

    public void Select()
    {
        // If already selected, do nothing.
        if (SelectionIndicator != null) return;

        // Instantiate the indicator at the character's position plus an offset
        GameObject indicatorPrefab = ResourceManager.LoadPrefab("Prefabs/Misc/SelectionIndicator");
        Vector3 indicatorPosition = transform.position + new Vector3(0f, SELECTION_INDICATOR_Y_OFFSET, 0f);
        SelectionIndicator = Instantiate(indicatorPrefab, indicatorPosition, Quaternion.identity, transform);
    }

    public void Deselect()
    {
        if (SelectionIndicator != null)
        {
            Destroy(SelectionIndicator);
            SelectionIndicator = null;
        }
    }
}

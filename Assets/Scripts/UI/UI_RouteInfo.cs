using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RouteInfo : MonoBehaviour
{
    [Header("Elements")]
    public TextMeshProUGUI PropulsionText;
    public TextMeshProUGUI WeightText;
    public TextMeshProUGUI DistanceText;

    public void UpdateInfo(Game game)
    {
        PropulsionText.text = $"{game.GetResourceChange(ResourceDefOf.PropulsionPower)} PP";
        WeightText.text = $"{game.GetTrainWeight()} kg";
        DistanceText.text = $"{game.GetTravelDistance()} km";
    }
}

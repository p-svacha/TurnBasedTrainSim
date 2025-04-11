using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Game Game { get; private set; }

    public Button EndTurnButton;
    public TextMeshProUGUI TimeText;
    public UI_RouteInfo RouteInfo;

    public void Init(Game game)
    {
        Game = game;
        EndTurnButton.onClick.AddListener(() => Game.StartTurnResolution());
        UpdateTimeText();
    }

    public void UpdateTurnResolution()
    {
        RouteInfo.UpdateInfo(Game);
    }

    public void UpdateTimeText()
    {
        TimeText.text = Game.Time.GetAsAbsoluteTimeString();
    }
}

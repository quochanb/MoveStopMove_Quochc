using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState currentState;

    private void Awake()
    {
        //xu tai tho
        int maxScreenHeight = 1920;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    private void Start()
    {
        ChangeGameState(GameState.MainMenu);
    }

    public void ChangeGameState(GameState newState)
    {
        this.currentState = newState;
    }

    public void OnMainMenu()
    {
        ChangeGameState(GameState.MainMenu);
    }

    public void OnGamePlay()
    {
        ChangeGameState(GameState.GamePlay);
    }

    public void OnGamePause()
    {
        ChangeGameState (GameState.GamePause);
    }

    public void OnRevive()
    {
        ChangeGameState(GameState.Revive);
    }

    public void OnFinish()
    {
        ChangeGameState(GameState.Finish);
    }
}

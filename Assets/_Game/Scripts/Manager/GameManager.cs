using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState currentState;
    private int levelNumber;
    private int playerCoin = 0;
    private void Awake()
    {
        //tranh viec nguoi choi cham da diem vao man hinh
        Input.multiTouchEnabled = false;
        //target frame rate ve 60 fps
        Application.targetFrameRate = 60;
        //tranh viec tat man hinh
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

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
        OnMainMenu();
    }

    public void ChangeGameState(GameState newState)
    {
        this.currentState = newState;
    }

    public void OnMainMenu()
    {
        ChangeGameState(GameState.MainMenu);

        int playerCoin = UserDataManager.Instance.GetUserCoin();
        UIManager.Instance.OpenUI<UIMainMenu>();
        UIManager.Instance.GetUI<UIMainMenu>().UpdateCoin(playerCoin);

        LevelManager.Instance.OnReset();
        levelNumber = UserDataManager.Instance.GetCurrentLevel();
        LevelManager.Instance.OnLoadLevel(levelNumber - 1);

        CameraFollow.Instance.ChangeCameraState(CameraState.MainMenu);
    }

    public void OnGamePlay()
    {
        ChangeGameState(GameState.GamePlay);
        CameraFollow.Instance.ChangeCameraState(CameraState.GamePlay);
    }

    public void OnGamePause()
    {
        ChangeGameState(GameState.GamePause);
    }

    public void OnRevive()
    {
        ChangeGameState(GameState.Revive);
        StartCoroutine(DelayOnRevive());
    }

    public void OnVictory()
    {
        ChangeGameState(GameState.Finish);
        StartCoroutine(DelayOnVictory());
    }

    public void OnFail()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIFail>();
        SoundManager.Instance.PlaySound(SoundType.Lose);
        ChangeGameState(GameState.Finish);
    }

    public void OnNextLevel()
    {
        levelNumber++;
        LevelManager.Instance.OnReset();
        LevelManager.Instance.OnLoadLevel(levelNumber - 1);
        UserDataManager.Instance.UpdateCurrentLevel(levelNumber);
        OnGamePlay();
    }

    public int GetCoin()
    {
        return playerCoin;
    }

    IEnumerator DelayOnRevive()
    {
        yield return Cache.GetWFS(1f);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIRevive>();
    }

    IEnumerator DelayOnVictory()
    {
        yield return Cache.GetWFS(1f);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIVictory>();
        CameraFollow.Instance.ChangeCameraState(CameraState.Victory);
        SoundManager.Instance.PlaySound(SoundType.Win);
    }
}

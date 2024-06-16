using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState currentState;
    private int levelNumber;

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
        EnterMainMenu();
    }

    public void ChangeGameState(GameState newState)
    {
        this.currentState = newState;
    }

    public bool IsGameState(GameState state)
    {
        return currentState == state;
    }

    public void EnterMainMenu()
    {
        ChangeGameState(GameState.MainMenu);

        int playerCoin = UserDataManager.Instance.GetUserCoin();
        UIManager.Instance.OpenUI<UIMainMenu>();
        UIManager.Instance.GetUI<UIMainMenu>().UpdateCoin(playerCoin);

        LevelManager.Instance.OnReset();
        levelNumber = UserDataManager.Instance.GetCurrentLevel();
        LevelManager.Instance.LoadLevel(levelNumber - 1);

        CameraFollow.Instance.ChangeCameraState(CameraState.MainMenu);
    }

    public void StartGamePlay()
    {
        ChangeGameState(GameState.GamePlay);
        CameraFollow.Instance.ChangeCameraState(CameraState.GamePlay);
    }

    public void PauseGame()
    {
        ChangeGameState(GameState.GamePause);
    }

    public void ShowRevivePopup()
    {
        ChangeGameState(GameState.Revive);
        StartCoroutine(DelayRevive());
    }

    public void HandleVictory()
    {
        ChangeGameState(GameState.Finish);
        StartCoroutine(DelayHandleVictory());
    }

    public void HandleFail()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIFail>().UpdateCoinDisplay(LevelManager.Instance.GetPlayerCoin());
        SoundManager.Instance.PlaySound(SoundType.Lose);
        ChangeGameState(GameState.Finish);
    }

    public void NextLevel()
    {
        levelNumber++;
        LevelManager.Instance.OnReset();
        LevelManager.Instance.LoadLevel(levelNumber - 1);
        UserDataManager.Instance.UpdateCurrentLevel(levelNumber);
        StartGamePlay();
    }

    IEnumerator DelayRevive()
    {
        yield return Cache.GetWFS(1f);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIRevive>();
    }

    IEnumerator DelayHandleVictory()
    {
        yield return Cache.GetWFS(1f);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIVictory>().UpdateCoinDisplay(LevelManager.Instance.GetPlayerCoin());
        CameraFollow.Instance.ChangeCameraState(CameraState.Victory);
        SoundManager.Instance.PlaySound(SoundType.Win);
    }
}

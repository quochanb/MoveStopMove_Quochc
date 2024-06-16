using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIVictory : UICanvas
{
    [SerializeField] private Button nextLevel;
    [SerializeField] private TextMeshProUGUI currentZone;
    [SerializeField] private TextMeshProUGUI nextZone;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI nextLevelText;

    private void Start()
    {
        nextLevel.onClick.AddListener(OnNextLevel);
        //update text
    }

    private void OnEnable()
    {
        int levelNumber = UserDataManager.Instance.GetCurrentLevel();
        UpdateTextCurrentZone(levelNumber);
        UpdateNextLevelText(levelNumber);
        UpdateTextNextZone(levelNumber);
    }

    public void UpdateCoinDisplay(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void UpdateTextCurrentZone(int currentLevel)
    {
        currentZone.text = $"ZONE: {currentLevel}";
    }

    private void UpdateTextNextZone(int currentLevel)
    {
        nextZone.text = $"ZONE: {currentLevel + 1}";
    }

    private void UpdateNextLevelText(int currentLevel)
    {
        nextLevelText.text = $"Play Zone {currentLevel + 1}";
    }

    //xu ly khi an Next btn
    private void OnNextLevel()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        Close(0);
        UIManager.Instance.OpenUI<UIGamePlay>();
        GameManager.Instance.NextLevel();
    }
}

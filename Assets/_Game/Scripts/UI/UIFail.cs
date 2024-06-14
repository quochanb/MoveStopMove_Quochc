using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFail : UICanvas
{
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI nextZone;
    [SerializeField] private TextMeshProUGUI currentZone;
    [SerializeField] private TextMeshProUGUI rankingText;
    [SerializeField] private TextMeshProUGUI attackerName;
    private void Start()
    {
        mainMenuBtn.onClick.AddListener(OnMainMenu);
    }

    private void OnEnable()
    {
        StartCoroutine(DelayActiveButton());

        int currentLevel = UserDataManager.Instance.GetCurrentLevel();
        UpdateTextCurrentZone(currentLevel);
        UpdateTextNextZone(currentLevel);
        UpdateRankingText(LevelManager.Instance.GetAliveEnemy());
        UpdateAttackerName(LevelManager.Instance.GetNameAttacker());
    }

    private void OnDisable()
    {
        mainMenuBtn.gameObject.SetActive(false);
    }

    public void OnMainMenu()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIMainMenu>();
        GameManager.Instance.OnMainMenu();
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

    private void UpdateRankingText(int number)
    {
        rankingText.text = $"#{number}";
    }

    private void UpdateAttackerName(string name)
    {
        attackerName.text = name;
    }

    IEnumerator DelayActiveButton()
    {
        yield return Cache.GetWFS(2f);
        mainMenuBtn.gameObject.SetActive(true);
    }
}

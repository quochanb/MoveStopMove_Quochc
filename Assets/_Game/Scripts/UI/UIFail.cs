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

    private void Start()
    {
        mainMenuBtn.onClick.AddListener(OnMainMenu);
    }

    private void OnEnable()
    {
        StartCoroutine(DelayActiveButton());
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

    IEnumerator DelayActiveButton()
    {
        yield return Cache.GetWFS(2f);
        mainMenuBtn.gameObject.SetActive(true);
    }
}

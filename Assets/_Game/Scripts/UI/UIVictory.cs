using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIVictory : UICanvas
{
    [SerializeField] private Button nextLevel;
    [SerializeField] private TextMeshProUGUI nextLevelText;

    private void Start()
    {
        nextLevel.onClick.AddListener(OnNextLevel);
        //update text
    }

    private void OnNextLevel()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        Close(0);
        UIManager.Instance.OpenUI<UIGamePlay>();
        GameManager.Instance.OnNextLevel();
    }

    private void UpdateNextLevelText(int currentLevel)
    {
        nextLevelText.text = currentLevel.ToString();
    }
}

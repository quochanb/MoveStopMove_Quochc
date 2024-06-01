using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVictory : UICanvas
{
    [SerializeField] private Button nextLevel;

    private void Start()
    {
        nextLevel.onClick.AddListener(OnNextLevel);
    }

    public void OnNextLevel()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIJoystick>();
        UIManager.Instance.OpenUI<UIGamePlay>();
    }
}

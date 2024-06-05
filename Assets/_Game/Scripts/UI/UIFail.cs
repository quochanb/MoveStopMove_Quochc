using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFail : UICanvas
{
    [SerializeField] private Button mainMenuBtn;

    private void Start()
    {
        mainMenuBtn.onClick.AddListener(OnMainMenu);
    }

    public void OnMainMenu()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIMainMenu>();
        GameManager.Instance.OnMainMenu();
        //UNDONE
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{
    [SerializeField] private Button settingBtn;

    private void Start()
    {
        settingBtn.onClick.AddListener(OnSetting);
    }

    public void OnSetting()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UISetting>();
        //UNDONE
    }
}

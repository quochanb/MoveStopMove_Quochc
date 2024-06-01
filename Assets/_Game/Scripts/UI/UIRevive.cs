using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRevive : UICanvas
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button reviveBtn;

    private void Start()
    {
        closeBtn.onClick.AddListener(OnClose);
        reviveBtn.onClick.AddListener(OnRevive);
    }

    public void OnClose()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIFail>();
        //UNDONE
    }

    public void OnRevive()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIJoystick>();
        UIManager.Instance.OpenUI<UIGamePlay>();
        //UNDONE
    }
}

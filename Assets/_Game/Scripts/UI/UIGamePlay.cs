using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : UICanvas
{
    [SerializeField] private Animator anim;
    [SerializeField] private Button settingBtn;
    [SerializeField] private TextMeshProUGUI aliveText;

    private string currentAnim;

    private void Start()
    {
        settingBtn.onClick.AddListener(OnSetting);
        ChangeAnim(Constants.ANIM_GL_OPEN);
    }

    public void OnSetting()
    {
        UIManager.Instance.CloseAll();
        ChangeAnim(Constants.ANIM_GL_CLOSE);
        UIManager.Instance.OpenUI<UISetting>();
        GameManager.Instance.OnGamePause();
        //UNDONE
    }

    public void UpdateAlive(int alive)
    {
        aliveText.text = "Alive: " + alive.ToString();
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }
}

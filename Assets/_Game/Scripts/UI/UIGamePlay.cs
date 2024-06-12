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
    }

    private void OnEnable()
    {
        StartCoroutine(DelayChangeAnim());
    }

    public void OnSetting()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        UIManager.Instance.OpenUI<UISetting>();
        ChangeAnim(Constants.ANIM_GL_CLOSE);
        GameManager.Instance.OnGamePause();
    }

    public void UpdateAlive(int alive)
    {
        aliveText.text = "Alive: " + alive.ToString();
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            if (currentAnim != null)
            {
                anim.ResetTrigger(currentAnim);
            }
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    IEnumerator DelayChangeAnim()
    {
        yield return Cache.GetWFS(0.5f);
        ChangeAnim(Constants.ANIM_GL_OPEN);
    }
}

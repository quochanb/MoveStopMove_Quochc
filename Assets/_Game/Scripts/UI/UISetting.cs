using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UICanvas
{
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button vibraBtn;
    [SerializeField] private Button homeBtn;
    [SerializeField] private Button continueBtn;

    [SerializeField] private Image soundOnIcon, soundOffIcon;
    [SerializeField] private Image vibraOnIcon, vibraOffIcon;

    [SerializeField] private TextMeshProUGUI onSoundText, offSoundText;
    [SerializeField] private TextMeshProUGUI onVibraText, offVibraText;

    private void Start()
    {
        soundBtn.onClick.AddListener(OnSoundBtnPress);
        vibraBtn.onClick.AddListener(OnVibraBtnPress);
        homeBtn.onClick.AddListener(OnHomeBtnPress);
        continueBtn.onClick.AddListener(OnContinueBtnPress);

        UpdateButtonIcon();
    }

    //xu ly bat-tat sound
    private void OnSoundBtnPress()
    {
        bool muted = PlayerPrefs.GetInt(Constants.P_PREF_MUTED, 0) == 1;
        PlayerPrefs.SetInt(Constants.P_PREF_MUTED, muted ? 0 : 1);
        UpdateButtonIcon();
    }

    //xu ly bat-tat vibration
    private void OnVibraBtnPress()
    {
        bool vibrated = PlayerPrefs.GetInt(Constants.P_PREF_VIBRATED, 0) == 1;
        PlayerPrefs.SetInt(Constants.P_PREF_VIBRATED, vibrated ? 0 : 1);
        UpdateButtonIcon();
    }

    //xu ly khi nhan nut home
    private void OnHomeBtnPress()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIMainMenu>();
        GameManager.Instance.OnMainMenu();
        //UNDONE
    }

    //xu ly khi nhan nut continue
    private void OnContinueBtnPress()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIJoystick>();
        UIManager.Instance.GetUI<UIGamePlay>().ChangeAnim(Constants.ANIM_GL_OPEN);
        //UIManager.Instance.OpenUI<UIGamePlay>();
        GameManager.Instance.OnGamePlay();
        //UNDONE
    }

    //cap nhat icon sound va vibration
    private void UpdateButtonIcon()
    {
        bool muted = PlayerPrefs.GetInt(Constants.P_PREF_MUTED, 0) == 1;
        bool vibrated = PlayerPrefs.GetInt(Constants.P_PREF_VIBRATED, 0) == 1;

        // Update icon & text sound
        soundOnIcon.enabled = !muted;
        soundOffIcon.enabled = muted;
        onSoundText.enabled = !muted;
        offSoundText.enabled = muted;

        // Update icon & text vibration
        vibraOnIcon.enabled = !vibrated;
        vibraOffIcon.enabled = vibrated;
        onVibraText.enabled = !vibrated;
        offVibraText.enabled = vibrated;
    }
}
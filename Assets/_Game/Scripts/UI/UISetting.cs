using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        AddButtonListenner(soundBtn, OnSoundBtnPress);
        AddButtonListenner(vibraBtn, OnVibraBtnPress);
        AddButtonListenner(homeBtn, OnHomeBtnPress);
        AddButtonListenner(continueBtn, OnContinueBtnPress);

        UpdateButtonIcon();
    }

    //add listenner cho button
    private void AddButtonListenner(Button button, UnityAction action)
    {
        button.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySound(SoundType.ButtonClick);
            action.Invoke();
        });
    }

    //xu ly bat-tat sound
    private void OnSoundBtnPress()
    {
        bool muted = PlayerPrefs.GetInt(Constants.P_PREF_MUTED, 0) == 1;
        PlayerPrefs.SetInt(Constants.P_PREF_MUTED, muted ? 0 : 1);
        SoundManager.Instance.SoundOff(!muted);
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
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIMainMenu>();
        GameManager.Instance.EnterMainMenu();
    }

    //xu ly khi nhan nut continue
    private void OnContinueBtnPress()
    {
        Close(0);
        UIManager.Instance.GetUI<UIGamePlay>().ChangeAnim(Constants.ANIM_GL_OPEN);
        GameManager.Instance.StartGamePlay();
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

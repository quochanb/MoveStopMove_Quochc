using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UICanvas
{
    [SerializeField] private Button wpShopBtn;
    [SerializeField] private Button skinShopBtn;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button vibraBtn;

    [SerializeField] private Image soundOnIcon, soundOffIcon;
    [SerializeField] private Image vibraOnIcon, vibraOffIcon;

    private void Start()
    {
        wpShopBtn.onClick.AddListener(OnOpenWeaponShop);
        skinShopBtn.onClick.AddListener(OnOpenSkinShop);
        playBtn.onClick.AddListener(OnPlayGame);
        soundBtn.onClick.AddListener(OnSoundBtnPress);
        vibraBtn.onClick.AddListener(OnVibraBtnPress);

        Load();
        UpdateButtonIcon();
    }

    public void OnOpenWeaponShop()
    {
        UIManager.Instance.OpenUI<UIWeaponShop>();
        //UNDONE
    }

    public void OnOpenSkinShop()
    {
        UIManager.Instance.OpenUI<UISkinShop>();
        //UNDONE
    }

    public void OnPlayGame()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIJoystick>();
        UIManager.Instance.OpenUI<UIGamePlay>();
    }

    //xu ly khi an nut sound
    public void OnSoundBtnPress()
    {
        //UNDONE
        bool muted = PlayerPrefs.GetInt(Constants.P_PREF_MUTED, 0) == 1;
        PlayerPrefs.SetInt(Constants.P_PREF_MUTED, muted ? 0 : 1);
        UpdateButtonIcon();
    }

    //xu ly khi an nut vibration
    public void OnVibraBtnPress()
    {
        //UNDONE
        bool vibrated = PlayerPrefs.GetInt(Constants.P_PREF_VIBRATED, 0) == 1;
        PlayerPrefs.SetInt(Constants.P_PREF_VIBRATED, vibrated ? 0 : 1);
        UpdateButtonIcon();
    }

    //load du lieu tu PlayerPref
    private void Load()
    {
        if (!PlayerPrefs.HasKey(Constants.P_PREF_MUTED))
        {
            PlayerPrefs.SetInt(Constants.P_PREF_MUTED, 0);
        }

        if (!PlayerPrefs.HasKey(Constants.P_PREF_VIBRATED))
        {
            PlayerPrefs.SetInt(Constants.P_PREF_VIBRATED, 0);
        }
    }

    //update trang thai cua button
    private void UpdateButtonIcon()
    {
        bool muted = PlayerPrefs.GetInt(Constants.P_PREF_MUTED, 0) == 1;
        bool vibrated = PlayerPrefs.GetInt(Constants.P_PREF_VIBRATED, 0) == 1;

        // Update sound icon
        soundOnIcon.enabled = !muted;
        soundOffIcon.enabled = muted;

        // Update vibration icon
        vibraOnIcon.enabled = !vibrated;
        vibraOffIcon.enabled = vibrated;
    }
}

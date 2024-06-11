using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private Animator anim;

    private string currentAnim;

    private void Start()
    {
        wpShopBtn.onClick.AddListener(OnOpenWeaponShop);
        skinShopBtn.onClick.AddListener(OnOpenSkinShop);
        playBtn.onClick.AddListener(OnPlayGame);
        soundBtn.onClick.AddListener(OnSoundBtnPress);
        vibraBtn.onClick.AddListener(OnVibraBtnPress);


        Load();
        UpdateButtonIcon();
        UpdateCoin(UserDataManager.Instance.GetUserCoin());
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    //xu ly khi an nut weapon shop
    private void OnOpenWeaponShop()
    {
        ChangeAnim(Constants.ANIM_MM_CLOSE);
        UIManager.Instance.OpenUI<UIWeaponShop>();
        //TODO: sound
    }

    //xu ly khi an nut skin shop
    private void OnOpenSkinShop()
    {
        ChangeAnim(Constants.ANIM_MM_CLOSE);
        UIManager.Instance.OpenUI<UISkinShop>();
        //TODO: sound + cameraState. Player change anim dance
    }

    //xu ly khi an nut Play
    private void OnPlayGame()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIJoystick>(); //FIXME
        UIManager.Instance.OpenUI<UIGamePlay>();
        GameManager.Instance.OnGamePlay();
        //TODO: sound + cameraState
    }

    //xu ly khi an nut sound
    private void OnSoundBtnPress()
    {
        //TODO: on/off sound
        bool muted = PlayerPrefs.GetInt(Constants.P_PREF_MUTED, 0) == 1;
        PlayerPrefs.SetInt(Constants.P_PREF_MUTED, muted ? 0 : 1);
        UpdateButtonIcon();
    }

    //xu ly khi an nut vibration
    private void OnVibraBtnPress()
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
}

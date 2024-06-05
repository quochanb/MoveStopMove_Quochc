using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponShop : UICanvas
{
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button closeBtn;

    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI weaponPrice;
    [SerializeField] private TextMeshProUGUI weaponProperty;
    [SerializeField] private TextMeshProUGUI playerCoin;

    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Image weaponImage;

    [SerializeField] Button[] buttonState;

    private Player player;
    private WeaponItem weaponItem;
    private int currentIndex = 0;


    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        ShowWeapon(currentIndex);
        ChangeButtonState(0); //Fix lai khi co userdata
        nextBtn.onClick.AddListener(OnNextButton);
        prevBtn.onClick.AddListener(OnPrevButton);
        closeBtn.onClick.AddListener(OnCloseButton);
    }

    public void ShowWeapon(int index)
    {
        if (index >= 0 && index < weaponData.weaponList.Count)
        {
            weaponItem = weaponData.weaponList[index];
            weaponName.text = weaponItem.name;
            weaponImage.sprite = weaponItem.sprite;
            weaponPrice.text = weaponItem.price.ToString();
            weaponProperty.text = weaponItem.wpProperty;
        }
    }

    public void ChangeButtonState(int index)
    {
        for (int i = 0; i < buttonState.Length; i++)
        {
            buttonState[i].gameObject.SetActive(false);
        }
        switch (index)
        {
            case 0:
                buttonState[0].gameObject.SetActive(true);
                buttonState[0].onClick.AddListener(OnBuyWeapon);
                break;
            case 1:
                buttonState[1].gameObject.SetActive(true);
                buttonState[1].onClick.AddListener(OnSelectWeapon);
                break;
            case 2:
                buttonState[2].gameObject.SetActive(true);
                buttonState[2].onClick.AddListener(OnCloseButton);
                break;
            default:
                break;
        }
    }

    public void UpdateCoin(int coin)
    {
        //int coinData (lay ra tu userdata)
        //playerCoin.text = (coinData - coint).ToString();
    }

    private void OnNextButton()
    {
        currentIndex++;
        if (currentIndex > weaponData.weaponList.Count - 1)
        {
            currentIndex = 0;
        }
        ShowWeapon(currentIndex);
        ChangeButtonState(0);
    }

    private void OnPrevButton()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = weaponData.weaponList.Count - 1;
        }
        ShowWeapon(currentIndex);
    }

    private void OnCloseButton()
    {
        Close(0);
        UIManager.Instance.GetUI<UIMainMenu>().ChangeAnim(Constants.ANIM_MM_OPEN);
    }

    private void OnBuyWeapon()
    {
        UpdateCoin(weaponItem.price);
        ChangeButtonState(1);
    }

    private void OnSelectWeapon()
    {
        player.ChangeWeapon(weaponItem.weaponType);
        ChangeButtonState(2);
    }
}

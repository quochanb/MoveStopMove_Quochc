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

    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Image weaponImage;

    [SerializeField] Button[] buttonState;

    private Player player;
    private WeaponItem weaponItem;
    private int currentIndex = 0;
    private UserData userData;


    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        userData = UserDataManager.Instance.userData;

        ShowWeapon(currentIndex);
        UpdateButtonState();

        nextBtn.onClick.AddListener(OnNextButton);
        prevBtn.onClick.AddListener(OnPrevButton);
        closeBtn.onClick.AddListener(OnCloseButton);
    }

    //show weapon
    public void ShowWeapon(int index)
    {
        if (index >= 0 && index < weaponData.weaponList.Count)
        {
            weaponItem = weaponData.weaponList[index];
            weaponName.text = weaponItem.name;
            weaponImage.sprite = weaponItem.sprite;
            weaponPrice.text = weaponItem.price.ToString();
            weaponProperty.text = weaponItem.wpProperty;

            UpdateButtonState();
        }
    }

    //update trang thai cac nut: buy, select, equipped
    public void UpdateButtonState()
    {
        for (int i = 0; i < buttonState.Length; i++)
        {
            buttonState[i].gameObject.SetActive(false);
            buttonState[i].onClick.RemoveAllListeners();
        }

        if (currentIndex < userData.weaponState.Count)
        {
            int state = userData.weaponState[currentIndex];
            switch (state)
            {
                //chua mua
                case 0:
                    buttonState[0].gameObject.SetActive(true);
                    buttonState[0].onClick.AddListener(OnBuyWeapon);
                    break;
                //da mua nhung chua trang bi
                case 1:
                    buttonState[1].gameObject.SetActive(true);
                    buttonState[1].onClick.AddListener(OnSelectWeapon);
                    break;
                //dang duoc trang bi
                case 2:
                    buttonState[2].gameObject.SetActive(true);
                    buttonState[2].onClick.AddListener(OnCloseButton);
                    break;
                default:
                    break;
            }
        }
    }

    //xu ly next button
    private void OnNextButton()
    {
        currentIndex++;
        if (currentIndex > weaponData.weaponList.Count - 1)
        {
            currentIndex = 0;
        }
        ShowWeapon(currentIndex);
    }

    //xu ly prev button
    private void OnPrevButton()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = weaponData.weaponList.Count - 1;
        }
        ShowWeapon(currentIndex);
    }

    //xu ly close button
    private void OnCloseButton()
    {
        Close(0);
        UIManager.Instance.GetUI<UIMainMenu>().ChangeAnim(Constants.ANIM_MM_OPEN);
    }

    //xu ly buy button
    private void OnBuyWeapon()
    {
        //ktra so tien cua nguoi choi co du mua item khong
        if (userData.coin >= weaponItem.price)
        {
            userData.coin -= weaponItem.price;
            UIManager.Instance.GetUI<UIMainMenu>().UpdateCoin(userData.coin);
            //update trang thai cua weapon
            UserDataManager.Instance.UpdateWeaponState(currentIndex, 1);
            UserDataManager.Instance.UpdateUserCoin(userData.coin);

            UpdateButtonState();
        }
    }

    //xu ly select button
    private void OnSelectWeapon()
    {
        //update trang thai cac vu khi ve 1 (da mua nhung chua trang bi)
        for(int i = 0; i < userData.weaponState.Count; i++)
        {
            if (userData.weaponState[i] == 2)
            {
                UserDataManager.Instance.UpdateWeaponState(i, 1);
            }
        }

        //update trang thai cua weapon hien tai
        UserDataManager.Instance.UpdateWeaponState(currentIndex, 2);
        UserDataManager.Instance.UpdateCurrentWeapon(currentIndex);
        player.ChangeWeapon(weaponItem.weaponType);
        UpdateButtonState();
    }
}

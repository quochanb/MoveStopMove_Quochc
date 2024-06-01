using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponShop : UICanvas
{
    [SerializeField] private Button nextBtn, prevBtn, closeBtn;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private TextMeshProUGUI weaponName, weaponPrice;
    [SerializeField] private TextMeshProUGUI playerCoin;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] Button[] buttonState;

    private WeaponItem weaponItem;

    private void Start()
    {
        SetUp();
        nextBtn.onClick.AddListener(OnNextButton);
        prevBtn.onClick.AddListener(OnPrevButton);
        closeBtn.onClick.AddListener(OnCloseButton);
    }

    public override void SetUp()
    {
        base.SetUp();
        weaponItem = weaponData.weaponList[0];
        weaponName.text = weaponItem.name;
        weaponSprite.sprite = weaponItem.sprite;
        weaponPrice.text = weaponItem.price.ToString();
    }

    private void OnNextButton()
    {
        
    }

    private void OnPrevButton()
    {
        
    }

    private void OnCloseButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<UIMainMenu>();
    }
}

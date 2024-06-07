using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISkinShop : UICanvas
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button selectBtn;
    [SerializeField] private Button unequipBtn;

    [SerializeField] private Button hatShop;
    [SerializeField] private Button pantShop;
    [SerializeField] private Button shieldShop;
    [SerializeField] private Button comboSetShop;

    [SerializeField] private HatData hatData;
    [SerializeField] private PantData pantData;
    [SerializeField] private ShieldData shieldData;

    [SerializeField] private ButtonItemUI itemUIPrefab;
    [SerializeField] private Transform parent;

    [SerializeField] private Image[] backgrounds;
    [SerializeField] private Button[] buttonState;
    [SerializeField] private TextMeshProUGUI propertyText;
    [SerializeField] private TextMeshProUGUI playerCoin;

    private ShopType currentShopType;
    private UserData userData;
    private int currentHatIndex = 0;
    private int currentPantIndex = 0;
    private int currentShieldIndex = 0;
    private int currentComboSkinIndex = 0;
    private ButtonItemUI currentSelectedItem;
    private List<ButtonItemUI> buttonItems = new List<ButtonItemUI>();

    private void Start()
    {
        userData = UserDataManager.Instance.userData;

        buyBtn.onClick.AddListener(OnBuyBtn);
        closeBtn.onClick.AddListener(OnCloseBtn);
        selectBtn.onClick.AddListener(OnSelectBtn);
        unequipBtn.onClick.AddListener(OnUnequipBtn);

        hatShop.onClick.AddListener(OnShowHatShop);
        pantShop.onClick.AddListener(OnShowPantShop);
        shieldShop.onClick.AddListener(OnShowShieldShop);
        comboSetShop.onClick.AddListener(OnShowComboSetShop);

        OnShowHatShop();
        UpdateCoinDisplay(userData.coin);
    }

    private void OnEnable()
    {
        ButtonItemUI.OnClicked += HandleItemSelected;
    }

    private void OnDisable()
    {
        ButtonItemUI.OnClicked -= HandleItemSelected;
    }

    //xu ly Hat button
    private void OnShowHatShop()
    {
        currentShopType = ShopType.HatShop;
        ClearShop();
        UpdateBackground(0);

        for (int i = 0; i < hatData.hatList.Count; i++)
        {
            ButtonItemUI item = Instantiate(itemUIPrefab, parent);
            item.SetData(hatData.hatList[i], i);
            item.OnInit();
            buttonItems.Add(item);
            propertyText.text = hatData.hatList[i].hatProperty;
        }

        UpdateButtonState();
    }

    //xu ly Pant button
    private void OnShowPantShop()
    {
        currentShopType = ShopType.PantShop;
        ClearShop();
        UpdateBackground(1);

        for (int i = 0; i < pantData.pantList.Count; i++)
        {
            ButtonItemUI item = Instantiate(itemUIPrefab, parent);
            item.SetData(pantData.pantList[i], i);
            item.OnInit();
            buttonItems.Add(item);
            propertyText.text = pantData.pantList[i].pantProperty;
        }

        UpdateButtonState();
    }

    //xu ly Shield button
    private void OnShowShieldShop()
    {
        currentShopType = ShopType.ShieldShop;
        ClearShop();
        UpdateBackground(2);

        for (int i = 0; i < shieldData.shieldList.Count; i++)
        {
            ButtonItemUI item = Instantiate(itemUIPrefab, parent);
            item.SetData(shieldData.shieldList[i], i);
            item.OnInit();
            buttonItems.Add(item);
            propertyText.text = shieldData.shieldList[i].shieldProperty;
        }

        UpdateButtonState();
    }

    //xu ly comboset button
    private void OnShowComboSetShop()
    {
        currentShopType = ShopType.ComboShop;
        ClearShop();
        UpdateBackground(3);
    }

    //xu ly close button
    private void OnCloseBtn()
    {
        Close(0);
        UIManager.Instance.GetUI<UIMainMenu>().ChangeAnim(Constants.ANIM_MM_OPEN);
    }

    //xu ly su kien khi nut duoc click
    private void HandleItemSelected(ButtonItemUI item)
    {
        currentSelectedItem = item;

        switch (currentShopType)
        {
            case ShopType.HatShop:
                currentHatIndex = item.GetItemIndex();
                break;
            case ShopType.PantShop:
                currentPantIndex = item.GetItemIndex();
                break;
            case ShopType.ShieldShop:
                currentShieldIndex = item.GetItemIndex();
                break;
            case ShopType.ComboShop:
                currentComboSkinIndex = item.GetItemIndex();
                break;
            default:
                break;
        }

        UpdateButtonState();
    }

    //xu ly buy button
    private void OnBuyBtn()
    {
        int itemIndex = currentSelectedItem.GetItemIndex();
        int itemPrice = currentSelectedItem.GetItemPrice();
        if (userData.coin >= itemPrice)
        {
            userData.coin -= itemPrice;
            UpdateCoinDisplay(userData.coin);
            
            UserDataManager.Instance.UpdateUserCoin(userData.coin);
            UserDataManager.Instance.UpdateItemState(currentShopType, itemIndex, 1);

            currentSelectedItem.UnLockItem();
            UpdateButtonState();
        }
    }

    //xu ly select button
    private void OnSelectBtn()
    {
        int index = currentSelectedItem.GetItemIndex();
        UnselectAllItems(currentShopType);
        UserDataManager.Instance.UpdateItemState(currentShopType, index, 2);
        UserDataManager.Instance.UpdateCurrentItem(currentShopType, index);
        currentSelectedItem.ActiveEquipped();
        UpdateButtonState();
    }

    //xu ly unequip button
    private void OnUnequipBtn()
    {
        int index = currentSelectedItem.GetItemIndex();
        UserDataManager.Instance.UpdateItemState(currentShopType, index, 1);
        UserDataManager.Instance.UpdateCurrentItem(currentShopType, -1);
        
        currentSelectedItem.DeactiveEquipped();
        UpdateButtonState();
    }


    //xoa item trong shop
    private void ClearShop()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        buttonItems.Clear();
    }

    public void UpdateCoinDisplay(int coin)
    {
        playerCoin.text = coin.ToString();
    }

    //update lai background item shop
    private void UpdateBackground(int index)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].gameObject.SetActive(true);
        }

        //tat background dang duoc chon
        backgrounds[index].gameObject.SetActive(false);
    }

    //update lai cac trang thai buy, select, unequip
    public void UpdateButtonState()
    {
        for (int i = 0; i < buttonState.Length; i++)
        {
            buttonState[i].gameObject.SetActive(false);
            buttonState[i].onClick.RemoveAllListeners();
        }

        int state;

        switch (currentShopType)
        {
            case ShopType.HatShop:
                state = userData.hatState[currentHatIndex];
                break;
            case ShopType.PantShop:
                state = userData.pantState[currentPantIndex];
                break;
            case ShopType.ShieldShop:
                state = userData.shieldState[currentShieldIndex];
                break;
            case ShopType.ComboShop:
                state = userData.comboSkinState[currentComboSkinIndex];
                break;
            default:
                state = -1;
                break;
        }

        if (state >= 0)
        {
            switch (state)
            {
                //chua mua
                case 0:
                    buttonState[0].gameObject.SetActive(true);
                    buttonState[0].onClick.AddListener(OnBuyBtn);
                    break;
                //da mua nhung chua trang bi
                case 1:
                    buttonState[1].gameObject.SetActive(true);
                    buttonState[1].onClick.AddListener(OnSelectBtn);
                    break;
                //dang trang bi nhung muon thao ra
                case 2:
                    buttonState[2].gameObject.SetActive(true);
                    buttonState[2].onClick.AddListener(OnUnequipBtn);
                    break;
                default:
                    break;
            }
        }
    }

    //goi khi item duoc select
    public void UnselectAllItems(ShopType shopType)
    {
        foreach (ButtonItemUI itemUI in buttonItems)
        {
            //lay ra state cua nut
            int state = UserDataManager.Instance.GetItemState(shopType, itemUI.GetItemIndex());
            //neu state = 2 (dang duoc trang bi) thi chuyen state = 1 va unequip
            if(itemUI != null && itemUI.GetShopType() == shopType && state == 2)
            {
                UserDataManager.Instance.UpdateItemState(shopType, itemUI.GetItemIndex(), 1);
                itemUI.DeactiveEquipped();
            }
        }
        UserDataManager.Instance.SaveUserData();
    }
}

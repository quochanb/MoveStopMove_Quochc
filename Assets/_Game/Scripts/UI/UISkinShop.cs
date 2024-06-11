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
    [SerializeField] private TextMeshProUGUI propertyItem;

    private Player player;
    private UserData userData;
    private ShopType currentShopType;
    private ButtonItemUI currentSelectedItem;
    private List<ButtonItemUI> buttonItems = new List<ButtonItemUI>();

    private void Awake()
    {
        userData = UserDataManager.Instance.userData;
        player = FindObjectOfType<Player>();

        buyBtn.onClick.AddListener(OnBuyBtn);
        closeBtn.onClick.AddListener(OnCloseBtn);
        selectBtn.onClick.AddListener(OnSelectBtn);
        unequipBtn.onClick.AddListener(OnUnequipBtn);

        hatShop.onClick.AddListener(OnShowHatShop);
        pantShop.onClick.AddListener(OnShowPantShop);
        shieldShop.onClick.AddListener(OnShowShieldShop);
        comboSetShop.onClick.AddListener(OnShowComboSetShop);

        OnShowHatShop();
    }

    private void OnEnable()
    {
        CameraFollow.Instance.ChangeCameraState(CameraState.Shop);
        player.ChangeAnim(Constants.ANIM_CHARSKIN);
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
            propertyItem.text = hatData.hatList[i].hatProperty;
        }
        SelectDefaultItem(ShopType.HatShop);
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
            propertyItem.text = pantData.pantList[i].pantProperty;
        }
        SelectDefaultItem(ShopType.PantShop);
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
            propertyItem.text = shieldData.shieldList[i].shieldProperty;
        }
        SelectDefaultItem(ShopType.ShieldShop);
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
        CameraFollow.Instance.ChangeCameraState(CameraState.MainMenu);
        player.ChangeAnim(Constants.ANIM_IDLE);
    }

    //xu ly su kien khi nut duoc click
    private void HandleItemSelected(ButtonItemUI item)
    {
        currentSelectedItem = item;
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
            UIManager.Instance.GetUI<UIMainMenu>().UpdateCoin(userData.coin);

            UserDataManager.Instance.UpdateUserCoin(userData.coin);
            UserDataManager.Instance.UpdateItemState(currentShopType, itemIndex, 1);

            currentSelectedItem.UnLockItem();
            UpdateButtonState();
        }
    }

    //xu ly select button
    private void OnSelectBtn()
    {
        if(currentSelectedItem != null)
        {
            currentSelectedItem.OnSelectButton();
        }
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
        UnequipItem(currentShopType);
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

        if (currentSelectedItem != null)
        {
            int index = currentSelectedItem.GetItemIndex();
            int state = UserDataManager.Instance.GetItemState(currentShopType, index);
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

    //goi khi nhan nut select
    public void UnselectAllItems(ShopType shopType)
    {
        foreach (ButtonItemUI itemUI in buttonItems)
        {
            //lay ra state cua nut
            int state = UserDataManager.Instance.GetItemState(shopType, itemUI.GetItemIndex());
            //neu state = 2 (dang duoc trang bi) thi chuyen state = 1 va unequip
            if (itemUI != null && itemUI.GetShopType() == shopType && state == 2)
            {
                UserDataManager.Instance.UpdateItemState(shopType, itemUI.GetItemIndex(), 1);
                itemUI.DeactiveEquipped();
            }
        }
    }

    //goi khi nhan nut unequip
    public void UnequipItem(ShopType shopType)
    {
        switch (shopType)
        {
            case ShopType.HatShop:
                player.ChangeHat(HatType.None);
                break;
            case ShopType.PantShop:
                player.ChangePant(PantType.None);
                break;
            case ShopType.ShieldShop:
                player.ChangeShield(ShieldType.None);
                break;
            default:
                break;
        }
    }

    //set item default khi bat dau vao shop
    private void SelectDefaultItem(ShopType shopType)
    {
        ButtonItemUI defaultItem = null;
        ButtonItemUI equippedItem = null;

        foreach (var item in buttonItems)
        {
            if (defaultItem == null)
            {
                defaultItem = item;
            }

            if (UserDataManager.Instance.GetItemState(shopType, item.GetItemIndex()) == 2)
            {
                equippedItem = item;
            }
        }

        currentSelectedItem = equippedItem != null ? equippedItem : defaultItem;

        if (currentSelectedItem != null)
        {
            currentSelectedItem.OnSelectButton();
        }
    }
}

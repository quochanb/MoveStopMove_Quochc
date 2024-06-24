using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private Button setFullShop;

    [SerializeField] private HatData hatData;
    [SerializeField] private PantData pantData;
    [SerializeField] private ShieldData shieldData;
    [SerializeField] private SetFullData setFullData;

    [SerializeField] private ButtonItemUI itemUIPrefab;
    [SerializeField] private Transform parent;

    [SerializeField] private Image[] backgrounds;
    [SerializeField] private Button[] buttonState;
    [SerializeField] private TextMeshProUGUI propertyItem;
    [SerializeField] private TextMeshProUGUI priceItem;

    private Player player;
    private UserData userData;
    private ShopType currentShopType;
    private ButtonItemUI currentSelectedItem;
    private List<ButtonItemUI> buttonItems = new List<ButtonItemUI>();

    private void Awake()
    {
        userData = UserDataManager.Instance.userData;


        AddButtonListenner(closeBtn, OnCloseBtn);
        AddButtonListenner(hatShop, OnShowHatShop);
        AddButtonListenner(pantShop, OnShowPantShop);
        AddButtonListenner(shieldShop, OnShowShieldShop);
        AddButtonListenner(setFullShop, OnShowSetFulltShop);
    }

    private void OnEnable()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        player.ChangeAnim(Constants.ANIM_CHARSKIN);
        CameraFollow.Instance.ChangeCameraState(CameraState.Shop);
        ButtonItemUI.OnClicked += HandleItemSelected;
        OnShowHatShop();
    }

    private void OnDisable()
    {
        ButtonItemUI.OnClicked -= HandleItemSelected;
    }

    private void Start()
    {
        player.ChangeAnim(Constants.ANIM_CHARSKIN);
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
        }
        SelectDefaultItem(ShopType.ShieldShop);
        UpdateButtonState();
    }

    //xu ly comboset button
    private void OnShowSetFulltShop()
    {
        currentShopType = ShopType.SetFullShop;
        ClearShop();
        UpdateBackground(3);

        for (int i = 0; i < setFullData.setFullList.Count; i++)
        {
            ButtonItemUI item = Instantiate(itemUIPrefab, parent);
            item.SetData(setFullData.setFullList[i], i);
            item.OnInit();
            buttonItems.Add(item);
        }
        SelectDefaultItem(ShopType.SetFullShop);
        UpdateButtonState();
    }

    //xu ly close button
    private void OnCloseBtn()
    {
        Close(0);
        UIManager.Instance.GetUI<UIMainMenu>().ChangeAnim(Constants.ANIM_MM_OPEN);
        CameraFollow.Instance.ChangeCameraState(CameraState.MainMenu);
        player.ChangeAnim(Constants.ANIM_IDLE);
        player.ChangeCurrentSkin();
    }

    //xu ly su kien khi nut duoc click
    private void HandleItemSelected(ButtonItemUI item)
    {
        currentSelectedItem = item;
        priceItem.text = currentSelectedItem.GetItemPrice().ToString();
        propertyItem.text = currentSelectedItem.GetItemProperty().ToString();
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
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
        if (currentSelectedItem != null)
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
                    AddButtonListenner(buyBtn, OnBuyBtn);
                    break;
                //da mua nhung chua trang bi
                case 1:
                    buttonState[1].gameObject.SetActive(true);
                    AddButtonListenner(selectBtn, OnSelectBtn);
                    break;
                //dang trang bi nhung muon thao ra
                case 2:
                    buttonState[2].gameObject.SetActive(true);
                    AddButtonListenner(unequipBtn, OnUnequipBtn);
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

    //thao trang bi
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
            case ShopType.SetFullShop:
                player.ChangeSetFull(SetFullType.Default);
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

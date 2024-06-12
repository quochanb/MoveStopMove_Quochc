using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItemUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image btnImage;
    [SerializeField] private Image btnLock;
    [SerializeField] private Image equipped;
    [SerializeField] private Image border;

    private HatItem hatItem;
    private PantItem pantItem;
    private ShieldItem shieldItem;

    //tao bien static de luu button hien tai duoc chon
    private static ButtonItemUI currentSelectedBtn;
    private Player player;
    private int itemIndex;
    private int itemPrice;
    private ShopType shopType;

    public static event Action<ButtonItemUI> OnClicked;

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        button.onClick.AddListener(OnSelectButton);
    }

    public void OnInit()
    {
        btnLock.gameObject.SetActive(true);
        equipped.gameObject.SetActive(false);
        border.enabled = false;
        UpdateState();
    }

    //set hat data
    public void SetData(HatItem hatItem, int index)
    {
        this.hatItem = hatItem;
        this.itemIndex = index;
        this.shopType = ShopType.HatShop;
        this.itemPrice = hatItem.hatPrice;
        btnImage.sprite = hatItem.hatSprite;
        UpdateState();
    }

    //set pant data
    public void SetData(PantItem pantItem, int index)
    {
        this.pantItem = pantItem;
        this.itemIndex = index;
        this.shopType = ShopType.PantShop;
        this.itemPrice = pantItem.pantPrice;
        btnImage.sprite = pantItem.pantSprite;
        UpdateState();
    }

    //set shield data
    public void SetData(ShieldItem shieldItem, int index)
    {
        this.shieldItem = shieldItem;
        this.itemIndex = index;
        this.shopType = ShopType.ShieldShop;
        this.itemPrice = shieldItem.shieldPrice;
        btnImage.sprite = shieldItem.shieldSprite;
        UpdateState();
    }

    //lay ra price cua item
    public int GetItemPrice()
    {
        return itemPrice;
    }

    //lay ra index cua item
    public int GetItemIndex()
    {
        return itemIndex;
    }

    public ShopType GetShopType()
    {
        return shopType;
    }

    public void UnLockItem()
    {
        btnLock.gameObject.SetActive(false);
    }

    public void ActiveEquipped()
    {
        equipped.gameObject.SetActive(true);
    }

    public void DeactiveEquipped()
    {
        equipped.gameObject.SetActive(false);
    }

    private void UpdateState()
    {
        int state = UserDataManager.Instance.GetItemState(shopType, itemIndex);
        if (state == 1)
        {
            UnLockItem();
        }
        else if (state == 2)
        {
            UnLockItem();
            ActiveEquipped();
        }
    }

    //xu ly khi an vao button
    public void OnSelectButton()
    {
        SoundManager.Instance.PlaySound(SoundType.ButtonClick);
        //tat border cua button
        if (currentSelectedBtn != null && currentSelectedBtn != this)
        {
            currentSelectedBtn.border.enabled = false;
        }
        //bat border cua button duoc chon
        border.enabled = true;
        //cap nhat lai nut duoc chon hien tai
        currentSelectedBtn = this;

        //phat di su kien khi an nut
        OnClicked?.Invoke(this);

        //doi trang bi khi an nut
        if (hatItem != null && player != null)
        {
            player.ChangeHat(hatItem.hatType);
        }
        if (pantItem != null && player != null)
        {
            player.ChangePant(pantItem.pantType);
        }
        if (shieldItem != null && player != null)
        {
            player.ChangeShield(shieldItem.shieldType);
        }
    }
}

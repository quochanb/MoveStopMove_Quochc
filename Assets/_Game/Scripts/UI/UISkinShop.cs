using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI propertyText;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
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

    private void OnBuyBtn()
    {
        
    }

    private void OnCloseBtn()
    {
        Close(0);
        UIManager.Instance.GetUI<UIMainMenu>().ChangeAnim(Constants.ANIM_MM_OPEN);
    }

    private void OnSelectBtn()
    {
        
    }

    private void OnUnequipBtn()
    {
        
    }

    //goi khi nhan button Hat shop
    private void OnShowHatShop()
    {
        ClearShop();
        UpdateBackground(0);
        for (int i = 0; i < hatData.hatList.Count; i++)
        {
            ButtonItemUI item = Instantiate(itemUIPrefab, parent);
            item.SetData(hatData.hatList[i]);
            item.OnInit();
            propertyText.text = hatData.hatList[i].hatProperty;
        }
    }

    //goi khi nhan button Pant shop
    private void OnShowPantShop()
    {
        ClearShop();
        UpdateBackground(1);
        for(int i = 0; i < pantData.pantList.Count; i++)
        {
            ButtonItemUI item = Instantiate(itemUIPrefab, parent);
            item.SetData(pantData.pantList[i]);
            item.OnInit();
            propertyText.text = pantData.pantList[i].pantProperty;
        }
    }

    //goi khi nhan button Shield shop
    private void OnShowShieldShop()
    {
        ClearShop();
        UpdateBackground(2);
        for(int i = 0; i< shieldData.shieldList.Count; i++)
        {
            ButtonItemUI item = Instantiate(itemUIPrefab, parent);
            item.SetData(shieldData.shieldList[i]);
            item.OnInit();
            propertyText.text = shieldData.shieldList[i].shieldProperty;
        }
    }

    private void OnShowComboSetShop()
    {
        ClearShop();
        UpdateBackground(3);
    }

    //xoa item trong shop
    private void ClearShop()
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    //cap nhat lai background item shop
    private void UpdateBackground(int index)
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].gameObject.SetActive(true);
        }

        //tat background dang duoc chon
        backgrounds[index].gameObject.SetActive(false);
    }
}

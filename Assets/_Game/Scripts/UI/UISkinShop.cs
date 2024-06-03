using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkinShop : UICanvas
{
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

    private void Start()
    {
        hatShop.onClick.AddListener(OnShowHatShop);
        pantShop.onClick.AddListener(OnShowPantShop);
        shieldShop.onClick.AddListener(OnShowShieldShop);
        comboSetShop.onClick.AddListener(OnShowComboSetShop);
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
        }
    }

    private void OnShowComboSetShop()
    {

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

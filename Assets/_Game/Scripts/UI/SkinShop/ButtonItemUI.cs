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

    private void Start()
    {
        button.onClick.AddListener(OnSelectButton);
    }

    public void OnInit()
    {
        btnLock.gameObject.SetActive(true);
        equipped.gameObject.SetActive(false);
        border.gameObject.SetActive(false);
    }

    public void SetData(HatItem hatItem)
    {
        this.hatItem = hatItem;
        btnImage.sprite = hatItem.hatSprite;
    }

    public void SetData(PantItem pantItem)
    {
        this.pantItem = pantItem;
        btnImage.sprite = pantItem.pantSprite;
    }

    public void SetData(ShieldItem shieldItem)
    {
        this.shieldItem = shieldItem;
        btnImage.sprite = shieldItem.shieldSprite;
    }

    //xu ly khi an vao button
    private void OnSelectButton()
    {
        border.gameObject.SetActive(true);
        //Instantiate
    }
}

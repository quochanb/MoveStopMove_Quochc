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
    }

    //set hat data
    public void SetData(HatItem hatItem)
    {
        this.hatItem = hatItem;
        btnImage.sprite = hatItem.hatSprite;
    }

    //set pant data
    public void SetData(PantItem pantItem)
    {
        this.pantItem = pantItem;
        btnImage.sprite = pantItem.pantSprite;
    }

    //set shield data
    public void SetData(ShieldItem shieldItem)
    {
        this.shieldItem = shieldItem;
        btnImage.sprite = shieldItem.shieldSprite;
    }

    //xu ly khi an vao button
    private void OnSelectButton()
    {
        //tat border cua button
        if (currentSelectedBtn != null && currentSelectedBtn != this)
        {
            currentSelectedBtn.border.enabled = false;
        }
        //bat border cua button duoc chon
        border.enabled = true;
        //cap nhat lai nut duoc chon hien tai
        currentSelectedBtn = this;

        //doi trang bi khi an nut
        if (hatItem != null)
        {
            player.ChangeHat(hatItem.hatType);
        }
        if (pantItem != null)
        {
            player.ChangePant(pantItem.pantType);
        }
        if (shieldItem != null)
        {
            player.ChangeShield(shieldItem.shieldType);
        }
    }
}

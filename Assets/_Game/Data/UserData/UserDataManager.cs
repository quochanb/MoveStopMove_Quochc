using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    public UserData userData;

    private void Awake()
    {
        LoadUserData();
    }

    //save data
    public void SaveUserData()
    {
        string json = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString(Constants.USERDATA_KEY, json);
        PlayerPrefs.Save();
    }

    //load data
    public void LoadUserData()
    {
        if (PlayerPrefs.HasKey(Constants.USERDATA_KEY))
        {
            string json = PlayerPrefs.GetString(Constants.USERDATA_KEY);
            userData = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            userData = new UserData();
            SaveUserData();
        }
    }

    //update name
    public void UpdateUserName(string newName)
    {
        userData.name = newName;
        SaveUserData();
    }

    //update coin
    public void UpdateUserCoin(int newCoin)
    {
        userData.coin = newCoin;
        SaveUserData();
    }

    //update state of weapon in weaponState list
    public void UpdateWeaponState(int index, int newState)
    {
        if (index >= 0 && index < userData.weaponState.Count)
        {
            userData.weaponState[index] = newState;
            SaveUserData();
        }
    }

    //update current weapon index
    public void UpdateCurrentWeapon(int newWeaponIndex)
    {
        userData.currentWeaponIndex = newWeaponIndex;
        SaveUserData();
    }

    //update item state
    public void UpdateItemState(ShopType shopType, int index, int newState)
    {
        switch(shopType)
        {
            case ShopType.HatShop:
                userData.hatState[index] = newState;
                break;
            case ShopType.PantShop:
                userData.pantState[index] = newState;
                break;
            case ShopType.ShieldShop:
                userData.shieldState[index] = newState;
                break;
            case ShopType.ComboShop:
                userData.comboSkinState[index] = newState;
                break;
            default:
                break;
        }
        SaveUserData();
    }

    //update current item index
    public void UpdateCurrentItem(ShopType shopType, int index)
    {
        switch (shopType)
        {
            case ShopType.HatShop:
                userData.currentHatIndex = index;
                break;
            case ShopType.PantShop:
                userData.currentPantIndex = index;
                break;
            case ShopType.ShieldShop:
                userData.currentShieldIndex = index;
                break;
            case ShopType.ComboShop:
                userData.currentComboSkinIndex = index;
                break;
            default:
                break;
        }
        SaveUserData();
    }

    public int GetItemState(ShopType shopType, int index)
    {
        switch (shopType)
        {
            case ShopType.HatShop:
                return userData.hatState[index];
            case ShopType.PantShop:
                return userData.pantState[index];
            case ShopType.ShieldShop:
                return userData.shieldState[index];
            case ShopType.ComboShop:
                return userData.comboSkinState[index];
            default:
                return -1;
        }
    }
}

[System.Serializable]
public class UserData
{
    public int coin;
    public int levelNumber;
    public string name;

    public int currentWeaponIndex;
    public int currentHatIndex;
    public int currentPantIndex;
    public int currentShieldIndex;
    public int currentComboSkinIndex;

    public List<int> weaponState;
    public List<int> hatState;
    public List<int> pantState;
    public List<int> shieldState;
    public List<int> comboSkinState;

    public UserData()
    {
        coin = 10000;
        levelNumber = 1;
        name = "YOU";

        currentWeaponIndex = 0;
        currentHatIndex = 0;
        currentPantIndex = 0;
        currentShieldIndex = 0;
        currentComboSkinIndex = 0;

        weaponState = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        hatState = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        pantState = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        shieldState = new List<int>() { 0, 0 };
        comboSkinState = new List<int>() { 0, 0, 0 };
    }
}

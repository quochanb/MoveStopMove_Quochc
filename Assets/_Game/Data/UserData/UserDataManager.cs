using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>
{
    public UserData userData;
    
    private string dataPath;

    public void SaveUserData()
    {
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(dataPath, json);
    }

    public void LoadUserData()
    {
        if(File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            userData = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            userData = new UserData();
            SaveUserData();
        }
    }
}

[System.Serializable]
public class UserData
{
    public int coin = 0;
    public string name = "YOU";

    public List<int> weaponState = new List<int>();
    public List<int> hatState = new List<int>();
    public List<int> pantState = new List<int>();
    public List<int> shieldState = new List<int>();

    public int currentWeapon = 0;
    public int currentHat = 0;
    public int currentPant = 0;
    public int currentShield = 0;
}

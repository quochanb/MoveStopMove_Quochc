using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="HatData", menuName ="LocalData/HatData", order = 1)]
public class HatData : ScriptableObject
{
    public List<HatItem> hatList;

    public Hat GetHat(HatType hatType)
    {
        return hatList[(int)hatType].hat;
    }
}

[System.Serializable]
public class HatItem
{
    public HatType hatType;
    public Hat hat;
    public Sprite hatSprite;
    public int hatPrice;
    public string hatProperty;
}

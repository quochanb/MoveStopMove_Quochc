using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="SetFullData", menuName ="LocalData/SetFullData", order = 5)]
public class SetFullData : ScriptableObject
{
    public List<SetFullItem> setFullList;

    public Material GetMaterialSkin(SetFullType setType)
    {
        return setFullList[(int)setType].setFullMaterial;
    }

    public GameObject GetWing(SetFullType setType)
    {
        return setFullList[(int)setType].wing;
    }

    public GameObject GetTail(SetFullType setType)
    {
        return setFullList[(int)setType].tail;
    }

    public GameObject GetHair(SetFullType setType)
    {
        return setFullList[(int)setType].hair;
    }

    public GameObject GetAccessory(SetFullType setType)
    {
        return setFullList[(int)setType].accessory;
    }
}

[System.Serializable]
public class SetFullItem
{
    public SetFullType setFullType;
    public Sprite sprite;
    public int price;
    public Material setFullMaterial;
    public GameObject wing;
    public GameObject tail;
    public GameObject hair;
    public GameObject accessory;
    public string property;
}

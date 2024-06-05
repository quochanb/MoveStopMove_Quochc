using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PantData", menuName = "LocalData/PantData", order = 2)]
public class PantData : ScriptableObject
{
    public List<PantItem> pantList;

    public Material GetPantMaterial(PantType pantType)
    {
        return pantList[(int)pantType].material;
    }
}

[System.Serializable]
public class PantItem
{
    public PantType pantType;
    public Material material;
    public Sprite pantSprite;
    public int pantPrice;
    public string pantProperty;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="SetFullData", menuName ="LocalData/SetFullData", order = 5)]
public class SetFullData : ScriptableObject
{
    public List<SetFullItem> setFullList;

}

[System.Serializable]
public class SetFullItem
{
    public SetFullType setFullType;
    public Sprite sprite;
    public int price;
    public string property;
}

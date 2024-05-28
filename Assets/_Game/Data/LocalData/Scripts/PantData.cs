using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="PantData", menuName ="LocalData/PantData", order = 2)]
public class PantData : ScriptableObject
{
    [SerializeField] List<Material> materialPantList;

    public Material GetPantMaterial(PantType pantType)
    {
        return materialPantList[(int) pantType];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ColorData", menuName ="LocalData/ColorData", order = 3)]
public class ColorData : ScriptableObject
{
    [SerializeField] List<Material> colorList;

    public Material GetMaterial(ColorType colorType)
    {
        return colorList[(int)colorType];
    }
}

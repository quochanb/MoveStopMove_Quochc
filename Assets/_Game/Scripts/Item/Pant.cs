using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pant : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinRenderer;
    [SerializeField] PantData pantData;

    public void ChangeMaterialPant(PantType pantType)
    {
        skinRenderer.material = pantData.GetPantMaterial(pantType);
    }
}

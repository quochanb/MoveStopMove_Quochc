using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI combatText;

    public void OnInit(int value)
    {
        combatText.text = $"+{value}";
        Invoke(nameof(OnDespawn), 1f);
    }

    public void OnDespawn()
    {
        if (combatText != null)
        {
            Destroy(combatText.gameObject);
        }
    }
}

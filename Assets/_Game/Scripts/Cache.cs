using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache : MonoBehaviour
{
    private static Dictionary<Collider, Character> characters = new Dictionary<Collider, Character>();

    public static Character GetCharacter(Collider collider)
    {
        if (!characters.ContainsKey(collider))
        {
            characters.Add(collider, collider.GetComponent<Character>());
        }
        return characters[collider];
    }

    private static Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWFS(float time)
    {
        if (!waitForSeconds.ContainsKey(time))
        {
            waitForSeconds.Add(time, new WaitForSeconds(time));
        }
        return waitForSeconds[time];
    }
}

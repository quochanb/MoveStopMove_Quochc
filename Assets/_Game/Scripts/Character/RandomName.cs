using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomName
{
    private static List<string> namesList;

    static RandomName()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(Constants.PATH_TEXTFILE);
        ReadTextFile(textAsset);
    }

    private static void ReadTextFile(TextAsset textAsset)
    {
        namesList = textAsset.text.Split('\n').ToList();
    }

    public static string GetRandomEnemyName()
    {
        return namesList[Random.Range(0, namesList.Count)];
    }
}

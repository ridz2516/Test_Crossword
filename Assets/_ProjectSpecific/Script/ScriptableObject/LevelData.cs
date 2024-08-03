using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData")]
public class LevelData : ScriptableObject
{
    public Sprite LevelBG;
    public Color LevelColor = Color.red; // this will be used for LineRenderer and Grid Color Per Level

    public char[] Alphabets;
    public List<string> TargetWords =  new List<string>();

    [Button]
    private void AutoGenerateWords()
    {
        if(Alphabets.Length > 2)
        {
            var wordsData = DictionaryChecker.Instance.ListofAllWords(Alphabets);
            TargetWords.Clear();

            int maxCount = wordsData.Count;
            maxCount = Mathf.Min(5, maxCount);

            wordsData.ShuffleList();

            for (int i = 0; i < maxCount; i++)
            {
                TargetWords.Add(wordsData[i].Word);
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GenerateWordCombinations : MonoBehaviour
{
    public TextAsset wordsFile; // Assign your words.txt file here
    public string outputFileName = "combinations.txt";
    public int minLength = 3;
    public int maxLength = 6;

    void Start()
    {
        if (wordsFile == null)
        {
            Debug.LogError("Words file not set.");
            return;
        }

        string[] words = wordsFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        HashSet<string> combinationsSet = new HashSet<string>();

        foreach (string word in words)
        {
            GetCombinations(word, minLength, maxLength, combinationsSet);
        }

        WriteCombinationsToFile(combinationsSet, outputFileName);
        Debug.Log($"Combinations written to {outputFileName}");
    }

    void GetCombinations(string word, int minLen, int maxLen, HashSet<string> combinationsSet)
    {
        for (int length = minLen; length <= maxLen; length++)
        {
            for (int i = 0; i <= word.Length - length; i++)
            {
                combinationsSet.Add(word.Substring(i, length));
            }
        }
    }

    void WriteCombinationsToFile(HashSet<string> combinationsSet, string fileName)
    {
        string path = Path.Combine(Application.dataPath, fileName);
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (string combination in combinationsSet)
            {
                writer.WriteLine(combination);
            }
        }
    }
}

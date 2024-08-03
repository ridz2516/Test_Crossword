using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryChecker : Singleton<DictionaryChecker>
{
    private Trie trie;

    public override void Start()
    {
        trie = new Trie();
        LoadWordList();
    }


    void LoadWordList()
    {
        TextAsset wordListText = Resources.Load<TextAsset>("wordlist"); // The name of your word list file without extension
        string[] lines = wordListText.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);

        Debug.Log("Total " + lines.Length);

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length > 0)
            {
                string word = parts[0].ToLower();
                string hint = parts.Length > 1 ? parts[1] : null;
                trie.Insert(word, hint);
            }
        }
    }

    public bool IsValidWord(string word, out string hint)
    {
        return trie.Search(word.ToLower(), out hint);
    }

    public WordData checkWord(string i_Target)
    {
        var hint = "";
        if (IsValidWord(i_Target,out hint))
        {
            return new WordData(i_Target, hint);
        }
        else
        {
            return null;
        }
    }


    [Button]
    public List<WordData> ListofAllWords(char[] i_CharList)
    {
        if (trie == null)
        {
            trie = new Trie();
            LoadWordList();
        }

        var testWorldList = GenerateWords(i_CharList, i_CharList.Length);
        List<WordData> worldLists = new List<WordData>();
        for (int i = 0; i < testWorldList.Count; i++)
        {
            WordData data = checkWord(testWorldList[i]);

            if(data != null)
            {
                worldLists.Add(data);
            }
        }

        return worldLists;
    }

    List<string> GenerateWords(char[] sampleChars, int maxLength)
    {
        List<string> result = new List<string>();

        for (int length = 3; length <= maxLength; length++)
        {
            GenerateWordsRecursive(sampleChars, length, "", result);
        }

        return result;
    }

    void GenerateWordsRecursive(char[] sampleChars, int length, string currentWord, List<string> result)
    {
        if (currentWord.Length == length)
        {
            result.Add(currentWord);
            return;
        }

        foreach (char c in sampleChars)
        {
            if (!currentWord.Contains(c.ToString()))
            {
                GenerateWordsRecursive(sampleChars, length, currentWord + c, result);
            }
        }
    }

}

using System.Collections.Generic;

public class TrieNode
{
    public Dictionary<char, TrieNode> children = new Dictionary<char, TrieNode>();
    public bool isWord;
    public string hint;
}

public class Trie
{
    private readonly TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }

    public void Insert(string word, string hint)
    {
        TrieNode node = root;
        foreach (char c in word)
        {
            if (!node.children.ContainsKey(c))
            {
                node.children[c] = new TrieNode();
            }
            node = node.children[c];
        }
        node.isWord = true;
        node.hint = hint;
    }

    public bool Search(string word, out string hint)
    {
        TrieNode node = root;
        foreach (char c in word)
        {
            if (!node.children.ContainsKey(c))
            {
                hint = null;
                return false;
            }
            node = node.children[c];
        }
        if (node.isWord)
        {
            hint = node.hint;
            return true;
        }
        hint = null;
        return false;
    }

}
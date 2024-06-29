using System;
using System.Collections.Generic;
using System.IO;

public class Dictionary
{
    private List<string>[] hashTable;
    private const int TABLE_SIZE = 100;

    public Dictionary()
    {
        hashTable = new List<string>[TABLE_SIZE];
        for (int i = 0; i < TABLE_SIZE; i++)
        {
            hashTable[i] = new List<string>();
        }
    }

    private int GetHash(string word)
    {
        return Math.Abs(word.ToLower().GetHashCode() % TABLE_SIZE);
    }

    public void AddWord(string word)
    {
        int hash = GetHash(word);
        if (!hashTable[hash].Contains(word.ToLower()))
        {
            hashTable[hash].Add(word.ToLower());
        }
    }

    public bool ContainsWord(string word)
    {
        int hash = GetHash(word);
        return hashTable[hash].Contains(word.ToLower());
    }

    public void LoadFromFile(string path)
    {
        foreach (string line in File.ReadLines(path))
        {
            AddWord(line.Trim());
        }
    }

    public void SaveToFile(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            for (int i = 0; i < TABLE_SIZE; i++)
            {
                foreach (string word in hashTable[i])
                {
                    writer.WriteLine(word);
                }
            }
        }
    }
}

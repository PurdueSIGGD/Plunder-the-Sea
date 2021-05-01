using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "StringIntTable", menuName = "ScriptableObjects/Table/StringInt", order = 1)]
public class StringIntTable : ScriptableObject
{
    // DO NOT MODIFY THE Single CLASS
    [System.Serializable]
    public class Pair
    {
        public string name;
        [TextArea]
        public string text;
        [TextArea]
        public string text2;
    }
    public Pair[] textSingles;

    public string getName(int i)
    {
        if (i >= textSingles.Length)
        {
            return null;
        }
        return textSingles[i].name;
    }

    public string getText(int i)
    {
        if (i >= textSingles.Length)
        {
            return null;
        }
        return textSingles[i].text;
    }

    public string getText2(int i)
    {
        if (i >= textSingles.Length)
        {
            return null;
        }
        return textSingles[i].text2;
    }

    [ContextMenu("generate defaults")]
    private void generateDefaults()
    {
        this.textSingles =
            (Enumerable.Range(0, 7).ToArray())
            .Select((c) => new Pair() { name = "", text = "", text2 = "" }).ToArray();
    }
}


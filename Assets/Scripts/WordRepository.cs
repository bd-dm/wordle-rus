using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class WordsData
{
    public string[] words;
}

public class WordRepository : MonoBehaviour
{
    [SerializeField] [Tooltip("The text asset with words for guess")]
    private TextAsset wordsForGuessAsset;
    [SerializeField] [Tooltip("The text asset with all words")]
    private TextAsset allWordsAsset;

    private List<string> _wordsForGuessList;
    private List<string> _allWordsList;

    void Awake()
    {
        _wordsForGuessList = GetWordsFromAsset(wordsForGuessAsset);
        _allWordsList = GetWordsFromAsset(allWordsAsset);
    }

    private List<string> GetWordsFromAsset(TextAsset asset)
    {
        WordsData wordsData = JsonUtility.FromJson<WordsData>(asset.text);
        return wordsData.words.ToList();
    }

    public string GetRandomWord()
    {
        return _wordsForGuessList[Random.Range(0, _wordsForGuessList.Count)];
    }

    public bool CheckIsWordExists(string word)
    {
        return _allWordsList.Contains(word.ToLower());
    }
}

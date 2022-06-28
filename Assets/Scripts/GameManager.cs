using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int WordLength = 5;
    private const int RowsAmount = 6;
    private const float LetterAnimationOffset = .5f;

    [SerializeField] [Tooltip("Word repository")]
    private WordRepository wordRepository;

    [SerializeField] 
    [Tooltip("Prefab for letter")]
    private Letter letterPrefab;
    
    [SerializeField] 
    [Tooltip("Grid parent")]
    private GridLayoutGroup gridLayout;

    private List<Letter> _letters;

    private string _guessedWord;
    private int _currentIndex = 0;
    private int _currentRow = 0;
    private char?[] _currentWord = new char?[WordLength];

    private string _currentWordString
    {
        get
        {
            StringBuilder word = new StringBuilder();
            for (var i = 0; i < WordLength; i++)
            {
                if (_currentWord[i] != null)
                {
                    word.Append(_currentWord[i].Value);
                }
            }
            
            return word.ToString();
        }
    }

    private void Awake()
    {
        SetupGrid();
    }

    private void Start()
    {
        _guessedWord = wordRepository.GetRandomWord();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ParseInput(Input.inputString);
        }
    }

    public void ParseInput(string value)
    {
        foreach (char c in value)
        {
            if (c == '\b')
            {
                DeleteLetter();
            }
            else if (c is '\n' or '\r')
            {
                GuessWord();
            }
            else
            {
                EnterLetter(c);
            }
        }
    }

    public void SetupGrid()
    {
        if (_letters == null)
        {
            _letters = new List<Letter>();
        }

        for (var i = 0; i < RowsAmount; i++)
        {
            for (var j = 0; j < WordLength; j++)
            {
                Letter letter = Instantiate(letterPrefab, gridLayout.transform);

                _letters.Add(letter);
            }
        }
    }

    public void EnterLetter(char c)
    {
        if (_currentIndex < WordLength)
        {
            _letters[(_currentRow * WordLength) + _currentIndex].EnterLetter(c);
            _currentWord[_currentIndex] = c;
            _currentIndex++;
        }
    }

    public void DeleteLetter()
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            _letters[(_currentRow * WordLength) + _currentIndex].DeleteLetter();
            _currentWord[_currentIndex] = null;
        }
    }

    private void ShakeRow()
    {
        for (var i = 0; i < WordLength; i++)
        {
            _letters[_currentRow * WordLength + i].Shake();
        }
    }

    private void SetRow(int rowNumber)
    {
        _currentRow = rowNumber;
        _currentIndex = 0;
        _currentWord = new char?[WordLength];
    }

    public void GuessWord()
    {
        bool isWordCorrect = wordRepository.CheckIsWordExists(_currentWordString);
        
        if (_currentIndex != WordLength || !isWordCorrect)
        {
            ShakeRow();
        }
        else
        {
            for (var i = 0; i < WordLength; i++)
            {
                LetterState state;
                
                if (_guessedWord[i] == _currentWordString[i])
                {
                    state = LetterState.Correct;
                } 
                else if (_guessedWord.Contains(_currentWordString[i]))
                {
                    state = LetterState.WrongLocation;
                }
                else
                {
                    state = LetterState.Incorrect;
                }

                StartCoroutine(
                    PlayLetterState(
                        i * LetterAnimationOffset, 
                        _currentRow * WordLength + i, 
                        state
                    )
                );
            }

            if (_currentRow != RowsAmount - 1)
            {
                SetRow(_currentRow + 1);
            }
        }
    }

    IEnumerator PlayLetterState(float offset, int index, LetterState letterState)
    {
        yield return new WaitForSeconds(offset);
        _letters[index].SetState(letterState);
    }
}

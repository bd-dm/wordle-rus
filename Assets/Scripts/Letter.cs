using System;
using TMPro;
using UnityEngine;

public class Letter : MonoBehaviour
{
    private readonly int _animatorResetTrigger = Animator.StringToHash("Reset");
    private readonly int _animatorShakeTrigger = Animator.StringToHash("Shake");
    private readonly int _animatorEnterLetterTrigger = Animator.StringToHash("EnterLetter");
    private readonly int _animatorStateParameter = Animator.StringToHash("State");

    private Animator _animator = null;
    private TextMeshProUGUI _text = null;

    public char? Entry { get; private set; } = null;
    public LetterState State { get; private set; } = LetterState.Idle;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        _text.text = null;
    }

    public void EnterLetter(char c)
    {
        Entry = c;
        _text.text = c.ToString().ToUpper();
        _animator.SetTrigger(_animatorEnterLetterTrigger);
    }

    public void DeleteLetter()
    {
        Entry = null;
        _text.text = null;
        _animator.SetTrigger(_animatorResetTrigger);
    }

    public void Shake()
    {
        _animator.SetTrigger(_animatorShakeTrigger);
    }

    public void SetState(LetterState state)
    {
        State = state;
        _animator.SetInteger(_animatorStateParameter, (int) state);
    }

    public void Reset()
    {
        State = LetterState.Idle;
        _animator.SetInteger(_animatorStateParameter, (int) LetterState.Idle);
        _animator.SetTrigger(_animatorResetTrigger);

        Entry = null;
        _text.text = null;
    }
}

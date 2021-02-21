using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private TMP_Text _score;

    private void OnEnable()
    {
        _game.NumberOfScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _game.NumberOfScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(uint score)
    {
        _score.text = score.ToString();
    }
}
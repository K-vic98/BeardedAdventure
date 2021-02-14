using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] LevelWallet _wallet;
    [SerializeField] Teleportation _portal;
    [SerializeField] TMP_Text _score;

    private int _scoreBuffer;

    private void OnEnable()
    {
        _wallet.ScoreChanged += OnScoreChanged;
        _portal.PlayerTeleportedToSpaceShip += OnPlayerTeleportedToSpaceShip;
    }

    private void OnDisable()
    {
        _wallet.ScoreChanged -= OnScoreChanged;
        _portal.PlayerTeleportedToSpaceShip -= OnPlayerTeleportedToSpaceShip;
    }

    private void OnScoreChanged(int score)
    {
        _score.text = score.ToString();
    }

    private void OnPlayerTeleportedToSpaceShip()
    {
 
    }
}
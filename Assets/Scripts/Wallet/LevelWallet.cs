using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelWallet : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] GameObject _coins;

    private Coin[] _coinsArray;
    private int _score;

    public event UnityAction<int> ScoreChanged;

    private void Awake()
    {
        _coinsArray = _coins.GetComponentsInChildren<Coin>();
    }

    private void OnEnable()
    {
        foreach (var coin in _coinsArray)
        {
            coin.CoinWasPickedUp += OnCoinWasPickedUp;
        }
        _player.PlayerDied += OnPlayerDied;
    }

    private void OnDisable()
    {
        foreach (var coin in _coinsArray)
        {
            coin.CoinWasPickedUp -= OnCoinWasPickedUp;
        }
        _player.PlayerDied -= OnPlayerDied;
    }

    private void OnCoinWasPickedUp(int numberOfScore)
    {
        _score += numberOfScore;
        ScoreChanged?.Invoke(_score);
    }

    private void OnPlayerDied()
    {
        _score = 0;
        ScoreChanged?.Invoke(_score);
    }
}
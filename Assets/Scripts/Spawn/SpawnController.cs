using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] Player _playerPrefab;
    [SerializeField] Portal _levelPortal;

    [SerializeField] GameObject _coins;

    private Coin[] _coinsArray;

    private void Awake()
    {
        _coinsArray = _coins.GetComponentsInChildren<Coin>();
    }

    private void OnEnable()
    {
        _player.PlayerDied += RestartLevel;
    }

    private void OnDisable()
    {
        _player.PlayerDied -= RestartLevel;
    }

    private void RestartLevel()
    {
        _player.PlayerDied -= RestartLevel;
        Destroy(_player.gameObject);

        ActiveCoins();
        _player = Instantiate(_playerPrefab, _levelPortal.transform.position, Quaternion.identity);
        _player.PlayerDied += RestartLevel;
    }

    private void ActiveCoins()
    {
        foreach (var coin in _coinsArray)
        {
            coin.gameObject.SetActive(true);
        }
    }
}
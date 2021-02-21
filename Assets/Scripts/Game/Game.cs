using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Player _trackedPlayer;
    [SerializeField] private Player _playerPrefab;
    [Header("Levels")]
    [SerializeField] private GameObject _levels;

    private Level[] _arrayOfLevels;
    private uint _numderOfLevel = 0;

    private ReceivingPortal _receivingPortal;
    private PortalTransmitter _portalTransmitter;

    private Coin[] _arrayOfCoins;

    private FinalLava _finalLava;
    private Vector3 _finalLavaStartPosition;

    private GameObject _finalLadders;
    private SpriteRenderer[] _finalLadderSpriteRenders;
    private BoxCollider2D[] _finalLadderboxColliders;

    private uint _levelscore;
    private uint _gameScore;

    public event UnityAction<uint> NumberOfScoreChanged;
    public event UnityAction<Player> PlayerBornOnLevel;

    private void Awake()
    {
        _arrayOfLevels = _levels.GetComponentsInChildren<Level>();
        SetGameValues();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void GoToNextLevel()
    {
        IncreaseLevelNumber();
        SetGameValues();
    }

    private void IncreaseLevelNumber()
    {
        _numderOfLevel += 1;
        if (_numderOfLevel == _arrayOfLevels.Length)
            _numderOfLevel = 0;
    }

    private void SetGameValues()
    {
        SetCurrentLevelsValues();
        _finalLavaStartPosition = _finalLava.transform.position;
        SetCurrentLaddersComponents();
    }

    private void SetCurrentLevelsValues()
    {
        if (_receivingPortal)
            UnsubscribeFromEvents();
        _receivingPortal = _arrayOfLevels[_numderOfLevel].ReceivingPortal;
        _portalTransmitter = _arrayOfLevels[_numderOfLevel].PortalTransmitter;
        _arrayOfCoins = _arrayOfLevels[_numderOfLevel].Coins.GetComponentsInChildren<Coin>();
        _finalLava = _arrayOfLevels[_numderOfLevel].FinalLava;
        _finalLadders = _arrayOfLevels[_numderOfLevel].FinalLadders;
        SubscribeToEvents();
    }
    
    private void SetCurrentLaddersComponents()
    {
        _finalLadderSpriteRenders = _finalLadders.GetComponentsInChildren<SpriteRenderer>();
        _finalLadderboxColliders = _finalLadders.GetComponentsInChildren<BoxCollider2D>();
    }
    
    private void OnPlayerDied()
    {
        _trackedPlayer.PlayerDied -= OnPlayerDied;
        Destroy(_trackedPlayer.gameObject);
        RestartLevel();
        _levelscore = _gameScore;
        NumberOfScoreChanged?.Invoke(_levelscore);
        _trackedPlayer = Instantiate(_playerPrefab, _receivingPortal.transform.position, Quaternion.identity);
        PlayerBornOnLevel?.Invoke(_trackedPlayer);
        _trackedPlayer.PlayerDied += OnPlayerDied;
    }

    private void RestartLevel()
    {
        ActiveCoins();
        DeactivateFinalLava();
        DeactivateLadders();
    }

    private void ActiveCoins()
    {
        foreach (var coin in _arrayOfCoins)
        {
            coin.gameObject.SetActive(true);
        }
    }

    private void DeactivateFinalLava()
    {
        _finalLava.StopLavaFlood();
        _finalLava.transform.position = _finalLavaStartPosition;
    }

    private void DeactivateLadders()
    {
        foreach (var finalLadderSpriteRender in _finalLadderSpriteRenders)
        {
            finalLadderSpriteRender.enabled = false;
        }
        foreach (var finalLadderboxCollider in _finalLadderboxColliders)
        {
            finalLadderboxCollider.enabled = false;
        }
    }

    private void OnCoinWasPickedUp(uint numberOfScore)
    {
        _levelscore += numberOfScore;
        NumberOfScoreChanged?.Invoke(_levelscore);
    }

    private void FreeseNumberOfScore()
    {
        _gameScore = _levelscore;
    }

    private void SubscribeToEvents()
    {
        _trackedPlayer.PlayerDied += OnPlayerDied;
        foreach (var coin in _arrayOfCoins)
        {
            coin.CoinWasPickedUp += OnCoinWasPickedUp;
        }
        _portalTransmitter.PlayerTeleportedToSpaceShip += RestartLevel;
        _portalTransmitter.PlayerTeleportedToSpaceShip += GoToNextLevel;
        _portalTransmitter.PlayerTeleportedToSpaceShip += FreeseNumberOfScore;
    }

    private void UnsubscribeFromEvents()
    {
        _trackedPlayer.PlayerDied -= OnPlayerDied;
        foreach (var coin in _arrayOfCoins)
        {
            coin.CoinWasPickedUp -= OnCoinWasPickedUp;
        }
        _portalTransmitter.PlayerTeleportedToSpaceShip -= RestartLevel;
        _portalTransmitter.PlayerTeleportedToSpaceShip -= GoToNextLevel;
        _portalTransmitter.PlayerTeleportedToSpaceShip -= FreeseNumberOfScore;
    }
}
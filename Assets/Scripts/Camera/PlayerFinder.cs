using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
public class PlayerFinder : MonoBehaviour
{
    [SerializeField] private Game _game;

    private Cinemachine.CinemachineVirtualCamera _cinemachine;

    private void Start()
    {
        _cinemachine = GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        _game.PlayerBornOnLevel += ChangeFollowPlayer;
    }

    private void OnDisable()
    {
        _game.PlayerBornOnLevel -= ChangeFollowPlayer;
    }

    private void ChangeFollowPlayer(Player player)
    {
        _cinemachine.Follow = player.gameObject.transform;
    }
}
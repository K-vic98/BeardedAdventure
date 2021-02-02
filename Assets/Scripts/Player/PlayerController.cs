using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _player;

    private void Start()
    {
        _player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            _player.MoveRight();
        }
        if (Input.GetKey(KeyCode.A))
        {
            _player.MoveLeft();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Jump();
        }
    }
}
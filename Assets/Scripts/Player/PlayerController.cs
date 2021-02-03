using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    private Movement _player;

    private void Start()
    {
        _player = GetComponent<Movement>();
    }

    private void Update()
    {
        _player.Move(Input.GetAxis("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
            _player.Jump();
    }
}
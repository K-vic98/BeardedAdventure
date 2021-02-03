using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Attack))]
public class PlayerController : MonoBehaviour
{
    private Movement _movement;
    private Attack _attack;

    private void Start()
    {
        _movement = GetComponent<Movement>();
        _attack = GetComponent<Attack>();
    }

    private void Update()
    {
        _movement.Move(Input.GetAxis("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
            _movement.Jump();

        if (Input.GetKeyDown(KeyCode.E))
            _attack.Throw(new Vector2(transform.localScale.x, 0));
    }
}
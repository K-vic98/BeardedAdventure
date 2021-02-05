using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _movement;
    private PlayerAttack _attack;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        _attack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        _movement.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        if (Input.GetKeyDown(KeyCode.Space))
            _movement.Jump();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 screenPosDepth = Input.mousePosition;
            screenPosDepth.z = 5.0f;
            _attack.Throw(Camera.main.ScreenToWorldPoint(screenPosDepth));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
//[RequireComponent(typeof(PlayerAttack))]
public class Player : MonoBehaviour
{
    private PlayerMovement _movement;
    //private PlayerAttack _attack;

    public event UnityAction PlayerDied;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
        //_attack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        _movement.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        if (Input.GetKeyDown(KeyCode.Space))
            _movement.Jump();
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Vector3 screenPosDepth = Input.mousePosition;
        //    screenPosDepth.z = 5.0f;
        //    _attack.Throw(Camera.main.ScreenToWorldPoint(screenPosDepth));
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy) || collision.TryGetComponent<Lava>(out Lava lava) || collision.TryGetComponent<Spike>(out Spike spike))
        {
            PlayerDied?.Invoke();
        }     
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    public event UnityAction PlayerDied;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        _playerMovement.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        if (Input.GetKeyDown(KeyCode.Space))
            _playerMovement.Jump();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy) || collision.TryGetComponent<Lava>(out Lava lava) || collision.TryGetComponent<Spike>(out Spike spike) || collision.TryGetComponent<Bullet>(out Bullet bullet))
        {
            PlayerDied?.Invoke();
        }
    }
}
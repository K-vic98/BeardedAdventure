using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction PlayerDied;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy) || collision.TryGetComponent<Lava>(out Lava lava))
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerDied?.Invoke();
    }
}
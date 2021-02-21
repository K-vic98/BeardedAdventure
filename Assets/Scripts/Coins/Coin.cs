using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private uint _numberOfScore;

    public event UnityAction<uint> CoinWasPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            CoinWasPickedUp?.Invoke(_numberOfScore);
            gameObject.SetActive(false);
        }
    }
}
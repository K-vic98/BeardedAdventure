using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private uint _numberOfScore;
    [SerializeField] private GameObject _effect;
    [SerializeField] private GameObject _audioEffect;

    public event UnityAction<uint> CoinWasPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            CoinWasPickedUp?.Invoke(_numberOfScore);
            Instantiate(_effect, transform.position, Quaternion.identity);
            Instantiate(_audioEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _numberOfScore;

    public event UnityAction<int> CoinWasPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            //Inventory.instance.AddPoints((uint)_numberOfScore);
            //Inventory gono = new Inventory();
            //Inventory.huys
            CoinWasPickedUp?.Invoke(_numberOfScore);
            gameObject.SetActive(false);
        }
    }
}
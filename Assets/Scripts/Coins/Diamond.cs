using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Diamond : MonoBehaviour
{
    [SerializeField] private FinalLadder _finalLadder;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _finalLadder.gameObject.SetActive(true);
        }
    }
}
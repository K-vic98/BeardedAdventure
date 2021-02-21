using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class SecretRoom : MonoBehaviour
{
    private TilemapRenderer _tilemapRenderer;

    private void Start()
    {
        _tilemapRenderer = gameObject.GetComponent<TilemapRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
            _tilemapRenderer.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
            _tilemapRenderer.enabled = true;
    }
}
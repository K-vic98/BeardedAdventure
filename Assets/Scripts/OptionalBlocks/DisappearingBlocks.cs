using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
[RequireComponent(typeof(TilemapCollider2D))]
public class DisappearingBlocks : MonoBehaviour
{
    [SerializeField] private Diamond _diamond;

    private TilemapRenderer _tilemapRenderer;
    private TilemapCollider2D _tilemapCollider2D;

    private void Start()
    {
        _tilemapRenderer = GetComponent<TilemapRenderer>();
        _tilemapCollider2D = GetComponent<TilemapCollider2D>();
    }

    private void OnEnable()
    {
        _diamond.DiamondWasPickedUp += DisableBlockDisplay;
    }

    private void OnDisable()
    {
        _diamond.DiamondWasPickedUp -= DisableBlockDisplay;
    }

    private void DisableBlockDisplay()
    {
        _tilemapRenderer.enabled = false;
        _tilemapCollider2D.enabled = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class FinalLadder : MonoBehaviour
{
    [SerializeField] private Diamond _diamond;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        _diamond.DiamondWasPickedUp += ActivateLadder;
    }

    private void OnDisable()
    {
        _diamond.DiamondWasPickedUp -= ActivateLadder;
    }

    private void ActivateLadder()
    {
        _spriteRenderer.enabled = true;
        _boxCollider2D.enabled = true;
    }
}
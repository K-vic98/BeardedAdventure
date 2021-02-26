using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class OptionalBlock : MonoBehaviour
{
    [SerializeField] private float _timeBeforeDisplayBlock;
    [SerializeField] private float _blockLifeTime;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private float _runningTime;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _runningTime += Time.deltaTime;
        if (_runningTime >= _timeBeforeDisplayBlock)
        {
            _spriteRenderer.enabled = true;
            _boxCollider.enabled = true;
        }
        if (_runningTime >= _blockLifeTime)
        {
            _spriteRenderer.enabled = false;
            _boxCollider.enabled = false;
            _runningTime = 0;
        }
    }
}
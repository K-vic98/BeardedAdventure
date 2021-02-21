using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BGParallax : MonoBehaviour
{
    [SerializeField] private Vector2 _mult;
    [SerializeField] private GameObject _trackObj;

    private Vector3 _anchorPointStartPos;
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _anchorPointStartPos = _trackObj.transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 distanceFromOriginToTrack = _trackObj.transform.position - _anchorPointStartPos;
        Vector2 shift = distanceFromOriginToTrack * _mult;
        _spriteRenderer.sharedMaterial.SetVector("_Offset", shift);
    }
}
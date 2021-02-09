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
    
    // Start is called before the first frame update
    void Start()
    {
        _anchorPointStartPos = _trackObj.transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distanceFromOriginToTrack = _trackObj.transform.position - _anchorPointStartPos;
        Vector2 shift = distanceFromOriginToTrack * _mult;
        _spriteRenderer.sharedMaterial.SetVector("_Offset", shift);
    }
}

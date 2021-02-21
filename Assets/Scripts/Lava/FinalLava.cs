using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLava : MonoBehaviour
{
    [SerializeField] private Diamond _diamond;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _target;

    private bool _isMoving;

    private void OnEnable()
    {
        _diamond.DiamondWasPickedUp += StartLavaFlood;
    }

    private void OnDisable()
    {
        _diamond.DiamondWasPickedUp -= StartLavaFlood;
    }

    private void Update()
    {
        if (_isMoving)
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }

    private void StartLavaFlood()
    {
        _isMoving = true;
    }

    public void StopLavaFlood()
    {
        _isMoving = false;
    }
}
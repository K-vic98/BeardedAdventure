using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    [SerializeField] private float _numberOfPoints;
    [SerializeField] private float _speed;
    [SerializeField] private float _shift;

    private float _runningTime;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        _runningTime = _shift;
    }

    private void Update()
    {
        _runningTime += Time.deltaTime * _speed;
        transform.position = new Vector3(_startPosition.x, _startPosition.y + ((Mathf.Sin(_runningTime) + 1) / 2) * _numberOfPoints, _startPosition.z);
    }
}
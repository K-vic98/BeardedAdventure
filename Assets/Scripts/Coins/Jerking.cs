using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jerking : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _numberOfPoints;

    private float _runningTime;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        _runningTime += Time.deltaTime * _speed;
        transform.position = new Vector3(_startPosition.x, _startPosition.y + Mathf.Sin(_runningTime) * _numberOfPoints, _startPosition.z);
    }
}
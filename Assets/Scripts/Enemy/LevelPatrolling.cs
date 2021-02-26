using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPatrolling  : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;

    private Transform[] _points;
    private int _currentPoint;

    private void Start()
    {
        _points = new Transform[_path.childCount];
        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i);
        }
    }

    public void MoveToPoint()
    {
        Transform target = _points[_currentPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        if (transform.position == target.position)
        {
            _currentPoint++;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            if (_currentPoint >= _points.Length)
                _currentPoint = 0;
        }
    }
}
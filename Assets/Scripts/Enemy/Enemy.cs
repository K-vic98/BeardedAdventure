using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelPatrolling))]
public class Enemy : MonoBehaviour
{
    private LevelPatrolling  _levelPatrolling;

    private void Start()
    {
        _levelPatrolling = GetComponent<LevelPatrolling >();
    }

    private void Update()
    {
        _levelPatrolling.MoveToPoint();
    }
}
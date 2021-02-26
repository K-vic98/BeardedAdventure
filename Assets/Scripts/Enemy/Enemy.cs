using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelPatrolling))]
public class Enemy : MonoBehaviour
{
    private LevelPatrolling  _enemyMovement;

    private void Start()
    {
        _enemyMovement = GetComponent<LevelPatrolling >();
    }

    private void Update()
    {
        _enemyMovement.MoveToPoint();
    }
}
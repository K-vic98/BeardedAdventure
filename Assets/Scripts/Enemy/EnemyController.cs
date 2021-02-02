using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyController : MonoBehaviour
{
    private EnemyMovement _enemy;

    private void Start()
    {
        _enemy = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        _enemy.Move();
    }
}
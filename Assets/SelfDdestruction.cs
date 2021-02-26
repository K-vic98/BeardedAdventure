using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDdestruction : MonoBehaviour
{
    [SerializeField] private float _lifeTime;

    private float _runningTime;

    private void Update()
    {
        _runningTime += Time.deltaTime;
        if (_runningTime >= _lifeTime)
            Destroy(gameObject);
    }
}
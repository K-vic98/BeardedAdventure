using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject _bulletTemplate;
    [SerializeField] private float _timeBetweenSpawns;

    private GameObject _target;
    private float _runningTime;

    private void Update()
    {
        _runningTime += Time.deltaTime;
        if (_runningTime >= _timeBetweenSpawns && _target != null)
        {
            GameObject bullet = Instantiate(_bulletTemplate, gameObject.transform.position, Quaternion.identity);
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            bullet.GetComponent<Bullet>().SetDirection(direction);
            _runningTime = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _target = null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject _weapon;
    [SerializeField] float _duration;
    [SerializeField] float _force;
    [SerializeField] float _scrollingForce;

    private GameObject boomerangInFlight;
    private Animator _animatorController;
    private Vector2 _diretion;
    private float _runningTime;
    private bool _isTrow;

    private void Start()
    {
        _animatorController = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isTrow)
        {
            _runningTime += Time.deltaTime;
            boomerangInFlight.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _runningTime * _scrollingForce));
            if (_runningTime <= _duration/2)
            {
                boomerangInFlight.transform.position += new Vector3(_diretion.x * _force * Time.deltaTime, _diretion.y *_force * Time.deltaTime, 0);
            }
            else
            {
                Vector3 difference = _weapon.transform.position - boomerangInFlight.transform.position;
                boomerangInFlight.transform.position += new Vector3(difference.x * _force * Time.deltaTime, difference.y * _force * Time.deltaTime, 0);
                if (difference.magnitude <= 0.2)
                    _runningTime = _duration;
            }
            if (_runningTime >= _duration)
            {
                Destroy(boomerangInFlight);
                _weapon.SetActive(true);
                _runningTime = 0;
                _isTrow = false;
                _animatorController.SetBool("isTrow", false);
            }
        }
    }

    public void Throw(Vector2 diretion)
    {
        if (_isTrow)
        {
            return;
        }
        boomerangInFlight = Instantiate(_weapon, _weapon.transform.position, Quaternion.identity);
        _weapon.SetActive(false);
        _diretion = diretion;
        _isTrow = true;
        _animatorController.SetBool("isTrow", true);
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCastDistance;
    [SerializeField] private float _obstacleCastDistance;
    [SerializeField] private LayerMask _contactFilter;
    
    private Rigidbody2D _rigidbody;
    private Animator _animatorController;
    private bool _isGround;
    private bool _onStairs;
    private float _gravityScale;
    private RaycastHit2D[] _hits = new RaycastHit2D[1];
    private ContactFilter2D _contactFilter2D;
    private AudioSource _jumpAudioEffect;

    private void Start()
    {
        _jumpAudioEffect = gameObject.GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animatorController = GetComponent<Animator>();
        _contactFilter2D.SetLayerMask(_contactFilter);
    }

    private void FixedUpdate()
    {
        _isGround = CheckLandAvailability();
        _animatorController.SetBool("isGround", _isGround);
        if (_onStairs && _rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += new Vector2(0, 0.1f);
        }
        if (_onStairs && _rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Ladder>(out Ladder ladder))
        {
            _onStairs = true;
            _gravityScale = _rigidbody.gravityScale;
            _rigidbody.gravityScale = 0;
            _animatorController.SetBool("onStairs", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Ladder>(out Ladder ladder))
        {
            _onStairs = false;
            _rigidbody.gravityScale = _gravityScale;
            _animatorController.SetBool("onStairs", false);
        }
    }

    private bool CheckLandAvailability()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _groundCastDistance, _contactFilter);
    }

    public void Move(Vector2 t)
    {
        t.x = Mathf.Clamp(t.x, -1, 1);
        t.y = Mathf.Clamp(t.y, -1, 1);
        if (!_onStairs)
            t.y = 0;
        var collisionCount = _rigidbody.Cast(t, _contactFilter2D, _hits, _obstacleCastDistance);
        if (collisionCount == 0)
            transform.Translate(t * (_speed * Time.deltaTime), Space.World);
        if (t.x != 0)
            transform.localScale = new Vector2(Mathf.Sign(t.x), transform.localScale.y);
        _animatorController.SetFloat("tx", t.x);
    }

    public void Jump()
    {
        if (_isGround)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumpAudioEffect.pitch = Random.Range(0.5f, 1.5f);
            _jumpAudioEffect.Play();
            _animatorController.SetTrigger("Jump");
        }
    }

    public void ClimbStairs()
    {
        if (_onStairs)
            transform.Translate(new Vector2(0, 10) * (_speed * Time.deltaTime), Space.World);
    }
}
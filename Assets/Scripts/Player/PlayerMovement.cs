using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _jumpForce = 10;

    private Rigidbody2D _rigidbody;
    private Animator _animatorController;
    private MoveState _moveState = MoveState.Idle;
    private DirectionState _directionState = DirectionState.Right;
    private float _walkTime = 0, _walkCooldown = 0.2f;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animatorController = GetComponent<Animator>();
        _directionState = transform.localScale.x > 0 ? DirectionState.Right : DirectionState.Left;
    }

    private void Update()
    {
        if (_moveState == MoveState.Jump)
        {
            if (_rigidbody.velocity == Vector2.zero)
            {
                Idle();
            }
        }
        else if (_moveState == MoveState.Walk)
        {
            transform.Translate((_directionState == DirectionState.Right ? Vector2.right : -Vector2.right) * (_speed * Time.deltaTime), Space.World);
            _walkTime -= Time.deltaTime;
            if (_walkTime <= 0)
            {
                Idle();
            }
        }
    }

    private void Idle()
    {
        _moveState = MoveState.Idle;
        _animatorController.Play("Idle");
    }

    public void MoveRight()
    {
        if (_moveState != MoveState.Jump)
        {
            _moveState = MoveState.Walk;
            if (_directionState == DirectionState.Left)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                _directionState = DirectionState.Right;
            }
            _walkTime = _walkCooldown;
            _animatorController.Play("Walk");
        }
    }

    public void MoveLeft()
    {
        if (_moveState != MoveState.Jump)
        {
            _moveState = MoveState.Walk;
            if (_directionState == DirectionState.Right)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                _directionState = DirectionState.Left;
            }
            _walkTime = _walkCooldown;
            _animatorController.Play("Walk");
        }
    }

    public void Jump()
    {
        if (_moveState != MoveState.Jump)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _moveState = MoveState.Jump;
            _animatorController.Play("Jump");
        }
    }

    enum DirectionState
    {
        Right,
        Left
    }

    enum MoveState
    {
        Idle,
        Walk,
        Jump
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _castDistance;
    [SerializeField] private LayerMask _contactFilter;

    private Rigidbody2D _rigidbody;
    private Animator _animatorController;
    private bool _isGround;
    private bool _onStairs;
    private float _gravityScale;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animatorController = GetComponent<Animator>();
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

    public void Move(Vector2 t)
    {
        t.x = Mathf.Clamp(t.x, -1, 1);
        t.y = Mathf.Clamp(t.y, -1, 1);
        if (!_onStairs)
        {
            t.y = 0;
        }
        transform.Translate(t* (_speed * Time.deltaTime), Space.World);
        if (t.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(t.x), transform.localScale.y);
        }
        _animatorController.SetFloat("tx", t.x);
    }

    public void ClimbStairs()
    {
        if (_onStairs)
            transform.Translate(new Vector2(0, 10) * (_speed * Time.deltaTime), Space.World);
    }

    public void Jump()
    {
        if (_isGround) 
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _animatorController.SetTrigger("Jump");
        }
    }

    private bool CheckLandAvailability()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _castDistance, _contactFilter);
    }
}
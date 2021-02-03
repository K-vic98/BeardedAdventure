using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _castDistance;

    private Rigidbody2D _rigidbody;
    private Animator _animatorController;
    private bool _isGround;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animatorController = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _isGround = CheckLandAvailability();
        _animatorController.SetBool("isGround", _isGround);
    }

    public void Move(float tx)
    {
        tx = Mathf.Clamp(tx, -1, 1);
        transform.Translate(new Vector2(tx, 0) * (_speed * Time.deltaTime), Space.World);
        if (tx != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(tx), transform.localScale.y);
        }
        _animatorController.SetFloat("tx", tx);
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
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, Vector2.down, _castDistance);
        return _hit;
    }
}
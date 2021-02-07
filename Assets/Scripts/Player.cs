using System;
using Data.Util;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private CapsuleCollider2D _collider;

    private bool _allowMove;
    private bool _isFloating;
    private bool _isOnStairs;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int Attack = Animator.StringToHash("attack");

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        ListenInputs();
    }

    private void ListenInputs()
    {
        ListenAttackInput();
        ListenJumpInput();
        ListenMovementInput();
    }

    private void ListenAttackInput()
    {
        if (!Input.GetButtonDown("Fire1")) return;
        _animator.SetTrigger(Attack);
    }

    private void ListenJumpInput()
    {
        if (!Input.GetButtonDown("Jump")) return;
        Jump();
    }
    
    private void Jump()
    {
        if (!_isFloating)
        {
            _rigidbody.AddForce(new Vector2(0f, _rigidbody.mass + _rigidbody.gravityScale * 130f), ForceMode2D.Impulse);
        }
    }

    private void ListenMovementInput()
    {
        var direction = Input.GetAxis("Horizontal");

        StairsMoveFix(direction);
        
        _rigidbody.velocity = new Vector2(direction * speed, _rigidbody.velocity.y);
        
        _animator.SetBool(IsRunning, direction != 0);

        if (direction == 0) return;
        
        transform.eulerAngles = new Vector3(0f, direction > 0f ? 0f : 180f, 0f);
    }

    private void StairsMoveFix(float direction)
    {
        // Ajuste pro personagem não escorregar na escada
        _allowMove = !_isOnStairs || direction != 0;
        _rigidbody.constraints = _allowMove ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            _isOnStairs = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isFloating = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Stairs"))
        {
            _isOnStairs = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isFloating = true;
        }
    }
}

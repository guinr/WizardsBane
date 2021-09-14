using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float maxHealth = 100f;
    public float health;
    public bool isAttacking;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private UserStatusController _userStatusController;

    private float _currentPosition;
    private float _previousPosition;
    private bool _allowMove;
    private bool _isFloating;
    private bool _isOnStairs;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Fall = Animator.StringToHash("fall");
    private static readonly int JumpAnimation = Animator.StringToHash("jump");

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _userStatusController = FindObjectOfType<Canvas>().GetComponent<UserStatusController>();
        health = maxHealth;
    }
    
    private void FixedUpdate()
    {
        ListenFalling();
    }
    
    private void ListenFalling()
    {
        if (_isOnStairs || !_isFloating) return;

        _currentPosition = _rigidbody.position.y;
        _animator.SetBool(Fall, _currentPosition < _previousPosition);
    }
    
    private void LateUpdate()
    {
        _previousPosition = _currentPosition;
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
            _rigidbody.AddForce(new Vector2(0f, (_rigidbody.mass + _rigidbody.gravityScale) * 20f), ForceMode2D.Impulse);
        }
    }

    private void ListenMovementInput()
    {
        if (isAttacking && !_isFloating) return;
        
        var direction = Input.GetAxis("Horizontal");

        StairsMoveFix(direction);
        
        _rigidbody.velocity = new Vector2(direction * speed, _rigidbody.velocity.y);
        
        _animator.SetBool(IsRunning, direction != 0);

        if (direction == 0) return;
        
        transform.eulerAngles = new Vector3(0f, direction > 0f ? 0f : 180f, 0f);
    }

    private void StairsMoveFix(float direction)
    {
        // Fix for character not slide in stairs
        _allowMove = !_isOnStairs || direction != 0;
        _rigidbody.constraints = _allowMove ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
    }

    private void OnTriggerEnter2D(Collider2D cldr)
    {
        if (cldr.gameObject.CompareTag("EnemyAttack"))
        {
            health -= 10;
            var healthPercentage = health / maxHealth;
            _userStatusController.UpdateHealth(healthPercentage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _animator.SetBool(JumpAnimation, false);
        _animator.SetBool(Fall, false);
        
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
            _animator.SetBool(JumpAnimation, true);
        }
    }

}

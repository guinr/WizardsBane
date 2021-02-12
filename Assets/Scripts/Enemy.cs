using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    
    private static readonly int HitAnimation = Animator.StringToHash("hit");
    private static readonly int DeathAnimation = Animator.StringToHash("death");
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D cldr)
    {
        if (cldr.gameObject.CompareTag("Attack"))
        {
            health -= 1;

            var pushDirection = cldr.gameObject.GetComponent<Collider2D>().transform.forward.z;
            _rigidbody.AddForce(new Vector2(pushDirection * (_rigidbody.mass + _rigidbody.gravityScale * 25f), 0f), ForceMode2D.Impulse);
            
            if (health == 0)
            {
                _animator.SetTrigger(DeathAnimation);
                Invoke(nameof(Destroy), 0.7f);
            }
            else
            {
                _animator.SetTrigger(HitAnimation);
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

using System;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    public float speed;
    public float moveTime;
    public float sightDistance;
    
    private bool _playerFound;
    private bool _dirRight;
    private float _timer;
    
    private static readonly int WalkingAnimation = Animator.StringToHash("walking");
    private static readonly int AttackingAnimation = Animator.StringToHash("attacking");
    
    protected override void OnEnemyStart()
    {
    }

    protected override void OnEnemyTriggerEnter2D()
    {
    }

    private void FixedUpdate()
    {
        var tsfPosition = transform.position;
        var offset = _dirRight ? 1 : -1;
        var start = new Vector2(tsfPosition.x + offset, tsfPosition.y);
        var rayDirection = SpriteRenderer.flipX ? Vector2.left : Vector2.right;
        
        var hit = Physics2D.Raycast(start, rayDirection, sightDistance);
        Debug.DrawRay(start, rayDirection * sightDistance, Color.white);

        _playerFound = hit.collider && hit.collider.CompareTag("Player");
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (IsBeingHit || IsDead) return;

        Animator.SetBool(WalkingAnimation, !_playerFound);
        Animator.SetBool(AttackingAnimation, _playerFound);
        
        if (_playerFound) return;
        
        if (_dirRight)
        {
            SpriteRenderer.flipX = false;
            transform.Translate(Vector2.right * (speed * Time.deltaTime));
        }
        else
        {
            SpriteRenderer.flipX = true;
            transform.Translate(Vector2.left * (speed * Time.deltaTime));
        }

        _timer += Time.deltaTime;
        
        if (!(_timer >= moveTime)) return;
        
        _dirRight = !_dirRight;
        _timer = 0f;
    }

}

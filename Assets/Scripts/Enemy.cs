using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int strength;
    
    private Rigidbody2D _rigidbody;
    protected Animator Animator;
    protected SpriteRenderer SpriteRenderer;

    private Color _defaultColor;
    private Material _whiteMaterial;
    private Material _customSpriteLitMaterial;

    protected bool IsBeingHit;
    protected bool IsDead;
    
    private static readonly int DeathAnimation = Animator.StringToHash("death");
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        SpriteRenderer = GetComponent<SpriteRenderer>();
        _customSpriteLitMaterial = SpriteRenderer.material;
        _defaultColor = SpriteRenderer.color;
        
        _whiteMaterial = Resources.Load<Material>("WhiteMaterial");
        
        OnEnemyStart();
    }

    protected abstract void OnEnemyStart();

    private void HitStart() {
        SpriteRenderer.color = Color.white;
        SpriteRenderer.material = _whiteMaterial;
        IsBeingHit = true;
    }
    
    private void HitEnd() {
        SpriteRenderer.color = _defaultColor;
        SpriteRenderer.material = _customSpriteLitMaterial;
        IsBeingHit = false;
    }

    private void OnTriggerEnter2D(Collider2D cldr)
    {
        if (!cldr.gameObject.CompareTag("Attack")) return;
        
        health -= 1;

        var pushDirection = cldr.gameObject.GetComponent<Collider2D>().transform.forward.z;
        _rigidbody.AddForce(new Vector2(pushDirection * (_rigidbody.mass + _rigidbody.gravityScale * 25f), 0f), ForceMode2D.Impulse);
        Invoke(nameof(HitStart), 0f);
        Invoke(nameof(HitEnd), 0.1f);

        if (health != 0) return;

        IsDead = true;
        
        Animator.SetTrigger(DeathAnimation);
        Invoke(nameof(Destroy), 0.7f);
        
        OnEnemyTriggerEnter2D();
    }
    
    protected abstract void OnEnemyTriggerEnter2D();

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

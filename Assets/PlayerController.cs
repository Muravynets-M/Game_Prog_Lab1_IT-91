using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public float Speed { get; set; } = 9f;
    public float JumpHeight { get; set; } = 6f;
    public float GroundDistance { get; set; } = 0.2f;

    private BoxCollider2D _boxCollider;
    private Vector2 _velocity = Vector2.zero;
    private bool _isGrounded = true;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _velocity.x = Speed * Input.GetAxis("Horizontal");

        if (_isGrounded)
        {
            _velocity.y = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                _velocity.y = JumpHeight;
            }
        }
        else
        {
            _velocity.y += (Time.deltaTime * Physics.gravity.y) / 2;
        }
        
        FlipSprite();
    }

    void FixedUpdate()
    {
        transform.Translate(_velocity * Time.deltaTime);
    }

    private void FlipSprite()
    {
        if (_velocity.x != 0)
        {
            _spriteRenderer.flipX = _velocity.x > 0;
        }
    }

    private void CheckCollision()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, _boxCollider.size, 0);
        
        foreach (Collider2D hit in hits)
        {
            if (hit == _boxCollider)
                continue;

            ColliderDistance2D colliderDistance = hit.Distance(_boxCollider);
            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && _velocity.y < 0)
                {
                    _isGrounded = true;
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Fields
    public float RunSpeed;
    private Rigidbody2D _rigidbody;
    public float JumpSpeed;
    public LayerMask GroundMask;
    private BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInputs = Input.GetAxis("Horizontal");
        _rigidbody.velocity = new Vector2(RunSpeed * horizontalInputs, _rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump")&& IsGrounded())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpSpeed);
        }

    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        Fruit potentialFruit = collision.gameObject.GetComponent<Fruit>();
        if (potentialFruit != null)
        {
            Destroy(potentialFruit.gameObject);
        }
    }

   bool IsGrounded()
    {
        return Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, GroundMask);
    }
}

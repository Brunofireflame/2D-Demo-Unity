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
    public float GrappleSpeed;
    private Collider2D GrappleObject;
    public float GrappleDistance;
    private Vector2 direction = Vector2.zero;

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
        

        //declares a direction from arrow keys
        if (Input.GetKeyDown("left")) direction = Vector2.left;
        else if (Input.GetKeyDown("right")) direction = Vector2.right;
        else if (Input.GetKeyDown("down")) direction = Vector2.down;
        else if (Input.GetKeyDown("up")) direction = Vector2.up;

        if (horizontalInputs != 0)
        _rigidbody.velocity = new Vector2(RunSpeed * horizontalInputs, _rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump")&& IsGrounded())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpSpeed);
        }

        //Grappling
        if (direction != Vector2.zero)
        {
            var grappleCast = Physics2D.BoxCast(_collider.bounds.center, new Vector2(0.2f,0.2f), 0, direction, GrappleDistance, GroundMask); 
            GrappleObject = grappleCast.collider;
            if (GrappleObject != null)
            {
                _rigidbody.velocity = direction * GrappleSpeed;
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GrappleObject == collision.collider)
        {
            GrappleObject = null;
            direction = Vector2.zero;
        }
    }

    bool IsGrounded()
	{
		return Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, GroundMask);
	}


    
}

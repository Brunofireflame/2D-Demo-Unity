using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Run,
        Jump,
        Grappling
    }
    //Fields
    public float RunSpeed;
    private Rigidbody2D _rigidbody;
    public float JumpSpeed;
    public LayerMask GroundMask;
    private BoxCollider2D _collider;
    public float GrappleSpeed;
    private Collider2D GrappleObject;
    public float GrappleDistance;
    private Vector2 GrappleDirection = Vector2.zero;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool isGrappling = false;
    private Animator _grappleAnimator;
    public GameObject GrappleGameObject;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _grappleAnimator = GrappleGameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInputs = Input.GetAxis("Horizontal");


        //gets a direction from arrow keys
        if (Input.GetKeyDown("space")) isGrappling = false;
        if (Input.GetKeyDown("left"))
        {
            isGrappling = true;
            GrappleDirection = Vector2.left;
        }
        else if (Input.GetKeyDown("right"))
        {
            isGrappling = true;
            GrappleDirection = Vector2.right;
        }
        else if (Input.GetKeyDown("down"))
        {
            isGrappling = true;
            GrappleDirection = Vector2.down;
        }
        else if (Input.GetKeyDown("up"))
        {
            isGrappling = true;
            GrappleDirection = Vector2.up;
        }

        if (horizontalInputs != 0)
        _rigidbody.velocity = new Vector2(RunSpeed * horizontalInputs, _rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump")&& IsGrounded())
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpSpeed);
        }

        if (isGrappling)
        {
            Grapple(GrappleDirection);
            //_grappleAnimator.SetInteger("GrappleState", 1);
        }
       // else _grappleAnimator.SetInteger("GrappleState", 0);
        UpdateAnimation(horizontalInputs);
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
            GrappleDirection = Vector2.zero;
            isGrappling = false;
        }
    }

    private bool IsGrounded()
	{
		return Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, 0.1f, GroundMask);
	}

   //animatior
    private void UpdateAnimation(float horizontalInput)
    {
        MovementState currentState;
        if (horizontalInput > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            _spriteRenderer.flipX = true;
        }

        if (isGrappling) currentState = MovementState.Grappling;
        else if (!IsGrounded()) currentState = MovementState.Jump;
        else if (horizontalInput != 0) currentState = MovementState.Run;
        else currentState = MovementState.Idle;
        _animator.SetInteger("MovementState", (int)currentState);
    }    


    private void Grapple(Vector2 direction)
    {
            var grappleCast = Physics2D.BoxCast(_collider.bounds.center, new Vector2(0.2f, 0.2f), 0, direction, GrappleDistance, 192);
        if (grappleCast.collider != null)
        {
            GrappleObject = grappleCast.collider;
            Box potentialBox = GrappleObject.gameObject.GetComponent<Box>();
            if (potentialBox != null)
            {
                Destroy(potentialBox.gameObject);
                Debug.Log("box hit");
            }
            else if (GrappleObject != null)
            {
                _rigidbody.velocity = direction * GrappleSpeed;
                Debug.Log(GrappleObject + "hit");
            }
            
        }
        else
        {
            isGrappling = false;
            Debug.Log("not grappling");
        }
    }
    

}

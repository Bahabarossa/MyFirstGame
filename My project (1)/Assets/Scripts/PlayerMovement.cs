using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D collid;
    private Animator anim;
    private float dirX=0f;
    [SerializeField]private float runnigSpeed;
    [SerializeField]private float jumpPower;
    [SerializeField]private LayerMask jumpableGround;

    private enum MovementState { idle,run,jump,fall}

    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        sprite= GetComponent<SpriteRenderer>();
        collid= GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    private void Update()
    {
        updateMovementState();
        updateAnimationState();

    }
    private void updateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.run;
            sprite.flipX=false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.run;
            sprite.flipX= true;
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > .1f)
        {
            state=MovementState.jump;
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovementState.fall;

        }

        anim.SetInteger("state", (int)state);
    }
    private bool IsGrounded()
    {
       return Physics2D.BoxCast(collid.bounds.center, collid.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    private void updateMovementState()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * runnigSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        if (Input.GetKey("s"))
        {
            rb.velocity = new Vector2(rb.velocity.x, -jumpPower);
        }
    }
}

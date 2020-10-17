using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    Animator animator;
    bool isJumping = false;
    float jumpVel = 0;
    public float maxJumpVel = 100;
    public float maxHorizVel = 10;
    public float hSpeed = 100;

    [Range(0.0001f, 1)]
    public float airFriction = 0.95f;
    public float groundFriction = 0.95f;
    bool isFliped = false;
    public bool isGrounded;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        jumpVel = 0;
    }

    void flip()
    {
        if(!isFliped)
        {
            isFliped = true;
            this.transform.localScale = new Vector3(-this.transform.localScale.x,this.transform.localScale.y,this.transform.localScale.z);
        }
    }

    void unflip()
    {
        if(isFliped)
        {
            isFliped = false;
            this.transform.localScale = new Vector3(-this.transform.localScale.x,this.transform.localScale.y,this.transform.localScale.z);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Init
        Vector2 velocity = Vector2.zero;
        float velDeltaX = 0;

        //Checking for ground
        RaycastHit2D[] castResult = Physics2D.BoxCastAll(this.transform.position+Vector3.down, new Vector2(1,2), 0, Physics2D.gravity, 1, LayerMask.GetMask("Ground"));
        RaycastHit2D minHit;
        isGrounded = false;
        
        for(int i = 0; i < castResult.Length; ++i)
        {
            if(castResult[i].transform.gameObject != this)
            {
                isGrounded = true;
                if(isGrounded)
                {
                    if(castResult.Length < castResult.Length){}//do smthng with cast results (if u want)
                }
                else
                {
                    minHit = castResult[i];
                    isGrounded = true;
                }
            }
        }

        //Processing inpus
        if(Input.GetAxisRaw("Vertical") > 0.25) // JUMP
        {
            if(isGrounded)
            {
                jumpVel = maxJumpVel;
                isJumping = true; // if holding jump btn
                animator.SetBool("isJumping", true);
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }
        else // not JUMP
        {
            if(isJumping)
            {
                //jumpVel = 1.5f*Physics2D.gravity.magnitude;
            }
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1) // moving horizontaly
        {
            animator.SetBool("isRunning", true);
            velDeltaX += Input.GetAxisRaw("Horizontal")*hSpeed;
            if(Input.GetAxisRaw("Horizontal") > 0.1)
                unflip();
            else
                flip();
        }
        else
        {
            animator.SetBool("isRunning", false);
            velDeltaX = rb.velocity.x;
        }

        //Forces
        if(isJumping)
        {
            this.jumpVel -= airFriction; // airFriction
            velocity += Vector2.up * Physics2D.gravity.magnitude * jumpVel;
        }
        else
        {
            jumpVel += Physics2D.gravity.magnitude;
            velocity -= Vector2.up * jumpVel;
        }

        
        //Applying resulting force
        if(Mathf.Abs(velDeltaX) > Mathf.Abs(velocity.x))
            velocity.x = velDeltaX;
        velocity.x *= groundFriction;
        rb.velocity = velocity;
    }
}

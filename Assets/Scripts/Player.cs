using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

// Reminder: In this script, the anim.SetBool strings are referencing the animator.

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float gravity = -10f;
    public float jumpHeight = 7f;
    public float centreRadius = .1f;

    private CharacterController2D controller;
    private SpriteRenderer rend;
    private Animator anim;
    private Vector3 velocity;
    bool isClimbing = false; // Is in climbing state

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centreRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal"); // Getting left & right (horizontal) input
        float inputV = Input.GetAxis("Vertical"); // Getting up & down (vertical) input

        
        if (!controller.isGrounded // If controller is NOT grounded...
            && !isClimbing) // AND is NOT climbing...
        {
            velocity.y += gravity * Time.deltaTime; // Apply delta to gravity
        }
        else
        {
            bool isJumping = Input.GetButtonDown("Jump"); // Get Spacebar input 

            if (isJumping) // If jump is pressed...
            {
                Jump(); // Make the controller jump
            }
        }
        
        anim.SetBool("IsGrounded", controller.isGrounded); // Setting IsGrounded animation to 
        anim.SetFloat("JumpY", velocity.y);

        Move(inputH);
        Climb(inputH, inputV);

        // If the character isn't climbing
        if (!isClimbing)
        {
            controller.move(velocity * Time.deltaTime); // Applies velocity to controller (to get it to move)
        }        
    }

    void Move(float inputH)
    {
        // Move the character controller left/right with input
        velocity.x = inputH * moveSpeed;

        // Set bool to true if left or right input is pressed
        bool isRunning = inputH != 0; // isRunning is either left (-1) or right (+1)

        // Animate the player to running if input is pressed
        anim.SetBool("IsRunning", isRunning);

        if (inputH < 0) // If left is pressed...
        {
            rend.flipX = true; // Flip the sprite. Without this, the sprite would just move left but still face the right.
        }

        if (inputH > 0) // If right is pressed...
        {
            rend.flipX = false; // Untick flipX, flipping the character back to facing right (its default).
        }
    }

    void Climb(float inputH, float inputV)
    {
        bool isOverLadder = false; // Is overlapping ladder       

        // Get a list of all hit objects overlapping point
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, centreRadius);
        // Loop through each point
        foreach (var hit in hits)
        {
            // If point overlaps a climbable object
            if(hit.tag == "Ladder")
            {
                // Player is overlapping ladder!
                isOverLadder = true;
                break; // Exit foreach loop
            }
        }

        // If is over climbable and input V (up and down) has been made
        if(isOverLadder && inputV != 0)
        {
            // Is Climbing
            isClimbing = true;
            velocity.y = 0; // Cancel Y velocity
        }

        // If NOT over ladder
        if (!isOverLadder)
        {
            // Not climbing anymore
            isClimbing = false;
        }

        // If is climbing
        if (isClimbing)
        {
            // Translate character up and down
            Vector3 inputDir = new Vector3(inputH, inputV);
            transform.Translate(inputDir * moveSpeed * Time.deltaTime);
        }

        anim.SetBool("IsClimbing", isClimbing); // Setting IsClimbing animation to isClimbing bool
        anim.SetFloat("ClimbSpeed", inputV); // Setting ClimbSpeed animation to up and down input
    }

    void Jump()
    {
        velocity.y = jumpHeight; // Set velocity's Y to height

        anim.SetTrigger("Jump");
    }
}

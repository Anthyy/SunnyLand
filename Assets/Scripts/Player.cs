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

        
        if (!controller.isGrounded) // If controller is NOT grounded
        {
            velocity.y += gravity * Time.deltaTime; // Apply delta to gravity
        }

        bool isJumping = Input.GetButtonDown("Jump"); // Get Spacebar input 

        if (isJumping) // If jump is pressed...
        {
            Jump(); // Make the controller jump
        }

        anim.SetBool("IsGrounded", controller.isGrounded);
        anim.SetFloat("JumpY", velocity.y);

        Move(inputH);
        Climb(inputV);

        controller.move(velocity * Time.deltaTime); // Applies velocity to controller (to get it to move)
    }

    void Move(float inputH)
    {
        // Move the character controller left/right with input
        velocity.x = inputH * moveSpeed;

        // Set bool to true if input is pressed
        bool isRunning = inputH != 0;

        // Animate the player to running if input is pressed
        anim.SetBool("IsRunning", isRunning);

        // Check if input is pressed
        if (isRunning)
        {
            // Flip character depending on left/right input
            rend.flipX = inputH < 0;
        }
    }

    void Climb(float inputV)
    {
        bool isOverLadder = false; // Is overlapping ladder       

        // Get a list of all hit objects overlapping point
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);
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

        // If is over climbable and input V has been made
        //  Is Climbing
        // If is climbing
        //  Perform logic for climbing
    }

    void Jump()
    {
        velocity.y = jumpHeight; // Set velocity's Y to height
    }
}

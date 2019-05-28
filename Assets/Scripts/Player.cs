using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float gravity = -10f;

    private CharacterController2D controller;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        Run(inputH);
        Climb(inputV);
    }

    void Run(float inputH)
    {
        controller.move(transform.right * inputH * moveSpeed * Time.deltaTime);
        bool isRunning = inputH != 0; // Detect movement (since 1 and -1 are movement, so if it's NOT 0, it's one of them)
        anim.SetBool("isRunning", inputH != 0);

        // Sneak Peak:
        // rend.flipX = inputH > 0
    }

    void Climb(float inputV)
    {

    }
}

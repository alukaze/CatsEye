using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;  // Reference to CharacterController2D
    public Animator animator;                 // Controls animation transitions
    public float runSpeed = 8f;               // Horizontal move speed

    private float horizontalMove = 0f;
    private bool jump = false;

    void Update()
    {
        // Get horizontal input (-1, 0, 1)
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Update movement animation
        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Detect jump input
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            if (animator != null)
                animator.SetBool("IsJumping", true);
        }

        // Reset jump animation when grounded
        if (controller.IsGrounded() && animator != null)
            animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        // Move player (horizontal + jump)
        controller.Move(horizontalMove, jump);
        jump = false;
    }
}

using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 400f;      // Vertical force applied when jumping
    [SerializeField] private LayerMask whatIsGround;      // Layer(s) considered as ground
    [SerializeField] private Transform groundCheck;       // Position used for ground detection
    [SerializeField] private float groundedRadius = 0.2f; // Ground check circle radius

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool wasGrounded = false;

    [Header("Events")]
    public UnityEvent OnLandEvent; // Invoked when the player lands

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        // Check if the player is standing on the ground
        wasGrounded = isGrounded;
        Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        isGrounded = hits.Length > 0;

        // Trigger landing event if just landed
        if (!wasGrounded && isGrounded)
            OnLandEvent.Invoke();
    }


    public void Move(float move, bool jump)
    {
        // Apply horizontal velocity directly
        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);

        // Jump if grounded
        if (jump && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isGrounded = false;
        }
    }

    public bool IsGrounded() => isGrounded;

    // Draws a wire circle in the editor for ground check visualization
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
        }
    }
}

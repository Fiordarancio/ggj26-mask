using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Mathf;

public class PlayerMovement : MonoBehaviour
{
  [Header("Movement")]
  private Vector2 moveInput = Vector2.zero; // used to store movements for fixed update
  private Vector3 moveDirection = Vector3.right;
  private bool isFlipped = false; // if sprite is oriented to left
  public float moveForce = 10f; // TODO mask specific
  public float jumpForce = 10f; // TODO mask specific
  public float maxHorizontalVelocity = 20f; // TODO mask specific

  public float dashForce = 10f;
  public float dashCooldown = 2f; // TODO mask specific
  public float dashDuration = 1.5f; // TODO mask specific
                                    // Dash timers
  private float lastDash = 0f;
  private float nextDashTime = 0f;

  [Header("Ground Checker")]
  public Transform groundCheck;
  public float groundDistance = 0.1f;
  public LayerMask groundMask;

  // Jumping
  private bool isGrounded = false;
  private bool isJumpingUp = false;

  // Getter to apply forces
  private Rigidbody rb;


  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Awake()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {
    isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
  }

  void FixedUpdate()
  {
    // Add movement if available
    if (moveInput.sqrMagnitude > 0.0001f)
      rb.AddForce(moveDirection * moveForce, ForceMode.Force);

    // Test if we are landing
    if (isJumpingUp)
      if (rb.linearVelocity.y <= 0)
        isJumpingUp = false;

    // Clamp horizontal velocity
    if (Abs(rb.linearVelocity.x) > maxHorizontalVelocity)
    {
      Debug.Log("Clamp velocity");
      rb.linearVelocity = new Vector3(
        rb.linearVelocity.x > 0 ? maxHorizontalVelocity : -maxHorizontalVelocity,
        rb.linearVelocity.y, rb.linearVelocity.z);
    }

  }

  public void onMove(InputAction.CallbackContext context)
  {
    // Don't move if still dashing
    if (Time.time < lastDash)
    {
      Debug.Log("Dashing, can't move");
      return;
    }

    Vector2 newMoveInput = context.ReadValue<Vector2>();

    // Compare with last input and flip if needed
    if (newMoveInput.x != 0)
    {
      if ((!isFlipped && (newMoveInput.x < moveInput.x)) || (isFlipped && (newMoveInput.x > moveInput.y)))
        Flip();
    }

    // Save input
    moveInput = newMoveInput;
  }

  private void Flip()
  {
    Debug.Log("Flip!");

    moveDirection.x *= -1;
    isFlipped = !isFlipped;
  }

  public void OnJump()
  {
    if (isGrounded && !isJumpingUp)
    {
      Debug.Log("OnJump");
      rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      isGrounded = false;
      isJumpingUp = true;
    }
  }

  public void OnDash()
  {
    // Don't dash if still in cooldown
    if (Time.time < nextDashTime) return;

    // Set cooldown timers
    nextDashTime = Time.time + dashCooldown;
    lastDash = Time.time + dashDuration;

    // TODO test forward as dash direction
    rb.AddForce(this.transform.forward * dashForce, ForceMode.Impulse);
  }

}
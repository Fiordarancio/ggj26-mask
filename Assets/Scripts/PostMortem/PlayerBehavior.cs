using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Mathf;

public class PlayerBehavior : MonoBehaviour
{
  [Header("Movement")]
  private Vector2 moveInput = Vector2.zero; // used to store movements for fixed update
  private Vector3 moveDirection = Vector3.right;
  private bool isFlipped = false; // if sprite is oriented to left
  public float moveForce = 10f; // TODO mask specific
  public float maxHorizontalVelocity = 20f; // TODO mask specific

  [Header("Dash")]
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

  [Header("Jump")]
  public float jumpForce = 10f; // TODO mask specific
  public float landForce = 20f;
  private bool isGrounded = false;
  private bool isJumpingUp = false;
  private bool isLandingTriggered = false;

  [Header("Mask")]
  public GameObject playerMask; // get info to send to Mask Manager
  // TODO mask specific data depending on MaskType/PlayerType
  private bool canThrowMask = true;
  // STATIC event to be fired on throw. Receives:
  // initial position, initial direction, maxdistance
  public static event Action<string, Vector3, Vector3, float> ThrowMask;

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
    if (isGrounded) isLandingTriggered = false;
  }

  // FixedUpdate is called once per tick (physics)
  void FixedUpdate()
  {
    // Add movement if available and not jumping up
    if (moveInput.sqrMagnitude > 0.0001f && !isJumpingUp)
      rb.AddForce(moveDirection * moveForce, ForceMode.Force);

    // Test if we are landing
    isJumpingUp = rb.linearVelocity.y > 0;

    // Clamp horizontal velocity
    if (Abs(rb.linearVelocity.x) > maxHorizontalVelocity)
    {
      rb.linearVelocity = new Vector3(
        rb.linearVelocity.x > 0 ? maxHorizontalVelocity : -maxHorizontalVelocity,
        rb.linearVelocity.y, rb.linearVelocity.z);
    }

  }

  // Subscribe/Unsubscribe to events
  private void OnEnable()
  {
    Mask.FallDown += onRecoverMask;
  }
  private void OnDisable()
  {
    Mask.FallDown -= onRecoverMask;
  }


  // Input Action functions
  public void onMove(InputAction.CallbackContext context)
  {
    // Don't move if still dashing
    if (Time.time < lastDash) return;

    Vector2 newMoveInput = context.ReadValue<Vector2>();

    // Compare with last input and flip if needed
    if (newMoveInput.x != 0)
      if ((!isFlipped && (newMoveInput.x < moveInput.x)) || (isFlipped && (newMoveInput.x > moveInput.y)))
        Flip();

    // Save input
    moveInput = newMoveInput;
  }

  private void Flip()
  {
    // Debug.Log("Flip!");

    moveDirection.x *= -1;
    isFlipped = !isFlipped;
  }

  public void OnJump()
  {
    if (isGrounded && !isJumpingUp)
    {
      rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      isGrounded = false;
      isJumpingUp = true;
    }
  }

  public void onLand()
  {
    // Don't dash already triggered
    if (isLandingTriggered) return;

    if (!isGrounded && !isJumpingUp) // is landing
    {
      // Dash down  
      rb.AddForce(Vector3.down * landForce, ForceMode.Impulse);
      isLandingTriggered = true;
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
    rb.AddForce(moveDirection * dashForce, ForceMode.Impulse);
  }

  public void onThrowMask()
  {
    if (canThrowMask)
    {
      Debug.Log("Can throw mask!");
      // Invoke action to MaskManager to spawn a Mask to throw
      ThrowMask?.Invoke(gameObject.name, playerMask.transform.position, moveDirection, 5f);
      
      // Set your mask inactive
      playerMask.SetActive(false);
      canThrowMask = false;
      Debug.Log($"Mask thrown by {gameObject.name}");
    }
    else
    {
      Debug.Log("No! Don't throw");
    }
  }

  public void onRecoverMask(string ownerName)
  {
    if (ownerName == gameObject.name)
    {
      playerMask.SetActive(true);
      canThrowMask = true;
    }
  }

}
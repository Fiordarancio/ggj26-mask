using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  [Header("Movement")]
  public float moveSpeed = 10f;
  public float jumpForce = 10f;

  [Header("Ground Check")]
  public Transform groundCheck;
  public float groundDistance = 3.0f;
  public LayerMask groundMask;

  private Rigidbody rb;
  private bool isGrounded = false;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {
    // Ground Check
    isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
  }

  public void Move(InputAction.CallbackContext context)
  {
    Vector2 moveInput = context.ReadValue<Vector2>();
    Vector3 currentVelocity = rb.linearVelocity;
    currentVelocity.x = moveInput.x * moveSpeed;
    rb.linearVelocity = currentVelocity;
  }
  
  public void Jump()
  {
    Debug.Log("Player is jumping");
  }

  public void ThrowMask()
  {
    Debug.Log("Player is throwing a mask");
    Transform maskTransform = transform.Find("Mask1");

    if (maskTransform != null)
    {
      MaskScript mask = maskTransform.gameObject.GetComponent<MaskScript>();

      if (mask.CanLaunch())
      {
        mask.LaunchMask();
      }
    }
  }

  public void Dash()
  {
    Debug.Log("Player is dashing");
  }

  public void PauseGame()
  {
    Debug.Log("Game is paused");
  }
  
  public void Parry()
  {
    Debug.Log("Player is parrying");
  }

}

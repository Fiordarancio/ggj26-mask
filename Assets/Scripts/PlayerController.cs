using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ground Check
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
    }

	private void MovePad(InputAction.CallbackContext context)
	{
		Vector2 moveInput = context.ReadValue<Vector2>(); // -1 o 1
        Vector3 velocity = rb.linearVelocity;
        moveInput.x = moveInput.x * moveSpeed;
		moveInput.y = isGrounded ? moveInput.y : moveInput.y * moveSpeed;
        rb.linearVelocity = velocity;
		Debug.Log("Player is moving!");
	}
    public void Move(InputAction.CallbackContext context){
		Vector2 moveInput = context.ReadValue<Keyboard>(); // -1 o 1
        Vector3 velocity = rb.linearVelocity;
        moveInput.x = moveInput.x * moveSpeed;
		moveInput.y = isGrounded ? moveInput.y : moveInput.y * moveSpeed;
        rb.linearVelocity = velocity;
		Debug.Log("Player is moving!");
    }
    public void Jump() {
		if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
      Debug.Log("Player is jumping");
    }
    public void ThrowMask() {
      Debug.Log("Player is throwing a mask");
      MaskScript mask = transform.Find("Mask").gameObject.GetComponent<MaskScript>();

      if (mask.CanLaunch())
      {
        mask.LaunchMask(Vector3.left);
      }
    }
    public void Dash() {
      Debug.Log("Player is dashing");
    }
    public void PauseGame() {
      Debug.Log("Game is paused");
    }
    public void Parry() {
      Debug.Log("Player is parrying");
    }
}

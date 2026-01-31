using UnityEngine.InputSystem;
using UnityEngine;
using System;

public enum Direction
{
    Left = -1,
    Right = 1
};

public class PlayerController : MonoBehaviour
{
	[Header("Movement")]
	public float 		moveSpeed = 10f;
	public float 		jumpForce = 10f;
	public float 		umpForce = 200f;

	public float		dashForce = 30f;

	public float		dashCooldown = 2f;

	public float		dashDuration = 1.5f;

	[Header("Ground Checker")]
	public 				Transform groundCheck;
	public float 		groundDistance = 0.1f;
	public 				LayerMask groundMask;

	public	InputActionReference _jumpAction;
	public	InputActionReference _dashAction;
	public	InputActionReference _parryAction;

	public bool			isGrounded = false;

	private				Rigidbody rb;

	// Last valid input direction: -1 = left, +1 = right
	private Direction 	facing = Direction.Right;

	private GameObject[] ownedMasks;
	public ParryArea 	parryArea;

	private float	lastDash = 0f;
	private float	nextDashTime = 0f;
  
  	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
	    rb = GetComponent<Rigidbody>();
		_jumpAction.action.performed += OnJump;
	    _parryAction.action.performed += TryParry;
		_dashAction.action.performed += OnDash;
	}

	void OnDestroy()
	{
		_jumpAction.action.performed -= OnJump;
	    _parryAction.action.performed -= TryParry;
		_dashAction.action.performed += OnDash;

	}

	  // Update is called once per frame
	void Update()
	{
	    isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
	}

	public void Move(InputAction.CallbackContext context)
	{
		if (Time.time < lastDash) return;

	    Vector2 moveInput = context.ReadValue<Vector2>();

		if (moveInput.x > 0f)
		        facing = Direction.Right;
		    else if (moveInput.x < 0f)
		        facing = Direction.Left;

		Vector3 currentVelocity = rb.linearVelocity;
		currentVelocity.x = moveInput.x * moveSpeed;
		rb.linearVelocity = currentVelocity;
	}
	
	public void OnJump(InputAction.CallbackContext ctx)
	{
		if (!isGrounded) return;

	    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
	    Debug.Log("Player JUMPS");
	}

	    // Update is called once per frame
	private void TryParry(InputAction.CallbackContext ctx)
	{
	    if (parryArea.canParry && !GameObject.ReferenceEquals(parryArea.curMask.GetComponent<MaskScript>().owner, gameObject))
	    {
	      parryArea.curMask.GetComponent<MaskScript>().CatchMask(gameObject);
	    }
	}
	
	public void ThrowMask()
	{
	    Transform maskTransform = null;

	    // Loop children to get mask
	    for(int i = 0; i < transform.childCount; i++)
	    {
	      Transform child = transform.GetChild(i);
	      if (child.tag == "Mask")
	      {
	        maskTransform = child.transform;
	        break;
	      }
	    }

	    if (maskTransform != null)
	    {
	      MaskScript mask = maskTransform.gameObject.GetComponent<MaskScript>();
	      if (mask.CanLaunch())
	      {
	        mask.LaunchMask();
	      }
	    }
	}

	public void OnDash(InputAction.CallbackContext ctx)
	{
	    if (Time.time < nextDashTime) return;
	
		nextDashTime = Time.time + dashCooldown;
		lastDash = Time.time + dashDuration;

	    Vector3 dashDirection = Vector3.right * (int)facing;
	    rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
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
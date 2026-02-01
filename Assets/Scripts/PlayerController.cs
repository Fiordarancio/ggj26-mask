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

	public float		dashForce = 10f;

	public float		dashCooldown = 2f;

	public float		dashDuration = 1.5f;

	[Header("Ground Checker")]
	public 				Transform groundCheck;
	public float 		groundDistance = 0.1f;
	public 				LayerMask groundMask;

	public	InputActionReference _jumpAction;
	public	InputActionReference _dashAction;
	public	InputActionReference _parryAction;
	public	InputActionReference _throwAction;
	public	InputActionReference _moveAction;

	public bool			isGrounded = false;

	private				Rigidbody rb;

	// Last valid input direction: -1 = left, +1 = right
	private Direction 	facing = Direction.Right;
  private bool jumpPressed = false;
  private bool movePressed = false;
  private Vector2 moveInput;

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
    _throwAction.action.performed += TryThrow;
    _moveAction.action.started += StartMove;
    _moveAction.action.performed -= StopMove;
	}

	void OnDestroy()
	{
		_jumpAction.action.performed -= OnJump;
    _parryAction.action.performed -= TryParry;
		_dashAction.action.performed -= OnDash;
    _throwAction.action.performed -= TryThrow;
    _moveAction.action.started -= StartMove;
    _moveAction.action.performed -= StopMove;

	}

	  // Update is called once per frame
	void Update()
	{
	    isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
	}

  void FixedUpdate()
  {
    if (jumpPressed)
    {
      rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
      jumpPressed = false;
    }

    if (movePressed)
    {
      Vector3 forceDir = new Vector3(moveInput.x, 0, 0);
      rb.linearVelocity = forceDir * moveSpeed * Time.deltaTime;
      movePressed = false;
    }
  }

	public void StartMove(InputAction.CallbackContext context)
	{
		//Controllo che impedisce input per lastDash secondi al fine di rendere il dash duraturo
		if (Time.time < lastDash) return; 

    movePressed = true;
    moveInput = context.ReadValue<Vector2>();

		//Salvataggio del last input per la direzione del dash
		if (moveInput.x > 0f)
    {
      facing = Direction.Right;
      if (transform.rotation.y == -180)
        transform.Rotate(0, 180, 0);
    }
    else if (moveInput.x < 0f)
    {
      facing = Direction.Left;
      if (transform.rotation.y == 0)
        transform.Rotate(0, 180, 0);
    }

    FlipChildren();
	}

  	public void StopMove(InputAction.CallbackContext context)
	{
    movePressed = false;
    moveInput = new Vector2(0, 0);
	}
	
    // Update is called once per frame
	public void OnJump(InputAction.CallbackContext ctx)
	{
		// se il player non tocca una superficie che fa parte del layer ground
		if (!isGrounded) return;
    jumpPressed = true;
	}
	
  private void TryParry(InputAction.CallbackContext ctx)
  {
    if (parryArea.canParry && 
      !GameObject.ReferenceEquals(parryArea.curMask.GetComponent<MaskScript>().owner, gameObject) &&
      parryArea.curMask.GetComponent<MaskScript>().launched)
    {
      parryArea.curMask.GetComponent<MaskScript>().CatchMask(gameObject);
    }
  }
  
  public void TryThrow(InputAction.CallbackContext ctx)
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
		// Cooldown per il dash modificabile dall'editor
	    if (Time.time < nextDashTime) return;
	
		// calcolo del tempo dall-ultimo dash e del tempo per il prossimo dash
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

  public int GetFacingDir()
  {
    if (facing == Direction.Left)
      return -1;
    else
      return 1;
  }

  private void FlipChildren()
  {
    for(int i=0; i<transform.childCount;i++)
    {
      Transform child = transform.GetChild(i);

      if ((child.localPosition.x >= 0 && facing == Direction.Right) || (child.localPosition.x < 0 && facing == Direction.Left))
      {
        child.localPosition = new Vector3(-child.localPosition.x, child.localPosition.y, child.localPosition.z);
      }
    }
  }
}
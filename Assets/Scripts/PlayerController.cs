using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  [Header("Movement")]
  public float moveSpeed = 10f;
  public float jumpForce = 200f;

  [Header("Ground Checker")]
  public Transform groundCheck;
  public float groundDistance = 0.1f;
  public LayerMask groundMask;

  public InputActionReference _jumpAction;

  public bool isGrounded= false;

  private Rigidbody rb;
  private GameObject[] ownedMasks;
  public ParryArea parryArea;
  public InputActionReference parryAction;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    rb = GetComponent<Rigidbody>();
	_jumpAction.action.performed += OnJump;
    parryAction.action.performed += TryParry;
  }

  void OnDestroy()
	{
		_jumpAction.action.performed -= OnJump;
    parryAction.action.performed -= TryParry;

	}

  // Update is called once per frame
  void Update()
  {
    isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
  }

  public void Move(InputAction.CallbackContext context)
  {
    Vector2 moveInput = context.ReadValue<Vector2>();
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

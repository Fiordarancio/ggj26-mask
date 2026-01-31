using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move() {
      Debug.Log("Player is moving");
    }
    public void Jump() {
      Debug.Log("Player is jumping");
    }
    public void ThrowMask() {
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

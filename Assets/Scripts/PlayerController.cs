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

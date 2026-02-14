using UnityEngine;
using System;

// I'm using a different approach for mask throwing in order to get them independent on the player.
// Basically, a mask is a projectile. We commonly handle it as a spawnable, disposable object.
// So, the player has a mask game object placeholder that gets inactive when "thrown".
// When we throw a mask we 1. instantiate a mask of the player's type, that moves in the player's
// forward direction 2. deactivate the player's mask placeholder
public class Mask : MonoBehaviour
{
  [Header("Info needed when created")]
  public string ownerName;
  public Vector3 initialPosition;
  public Vector3 direction;
  public float maxDinstance = 5f; // TODO mask specific
  public float velocity = 1f; // TODO mask specific

  // Event to notify the mask can return to the player
  public static event Action<string> FallDown;

  // Update is called once per frame
  void Update()
  {
    float currentDinstance = Vector3.Distance(gameObject.transform.position, initialPosition);
    if (currentDinstance > maxDinstance)
    {
      // Drop or flash return to player (should get its position though)
      Debug.Log("Dinstance overcome");
      velocity = 0f;
      destroyMask();
    }

    // Move
    gameObject.transform.Translate(direction * velocity * Time.deltaTime);
  }


  public void initialize(string owname, Vector3 pos, Vector3 dir, float dist)
  {
    ownerName = owname;
    initialPosition = pos;
    direction = dir;
    maxDinstance = dist;
  }

  public void destroyMask()
  {
    FallDown?.Invoke(ownerName);
    // Start animation then...
    Destroy(gameObject);
  }
}

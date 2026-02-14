using UnityEngine;

public class MaskManager : MonoBehaviour {
  
  [Header("Spawn data")]
  public GameObject maskPrefab;

  private void OnEnable() {
    PlayerBehavior.ThrowMask += spawnAndThrowMask;
  }
  private void OnDisable() {
    PlayerBehavior.ThrowMask -= spawnAndThrowMask;
  }

  private void spawnAndThrowMask(
    string ownerName,
    Vector3 initialPosition,
    Vector3 ownerDirection,
    float maxDinstance
  )
  {
    Debug.Log($"{ownerName} throws mask from {initialPosition} towards {ownerDirection} until {maxDinstance}");
    GameObject spawnMask = Instantiate(maskPrefab, initialPosition, Quaternion.identity);
    Mask m = spawnMask.GetComponent<Mask>();
    if (m)
      m.initialize(ownerName, initialPosition, ownerDirection, maxDinstance);
  }
}
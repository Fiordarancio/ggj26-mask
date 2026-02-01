using UnityEngine;

public class CameraTarget : MonoBehaviour
{
  [Header("Camera Target players")]
  public GameObject player1;
  public GameObject player2;

  // Update is called once per frame
  void Update()
  {
    // The cameratarget position takes the median position of both players
    if (player1 != null && player2 != null)
    {
      Vector3 midPoint = (player1.transform.position + player2.transform.position) / 2;
      transform.position = new Vector3(midPoint.x, midPoint.y, transform.position.z);
    }
    else if (player1 != null)
    {
      transform.position = new Vector3(player1.transform.position.x, player1.transform.position.y, transform.position.z);
    }
    else if (player2 != null)
    {
      transform.position = new Vector3(player2.transform.position.x, player2.transform.position.y, transform.position.z);
    }

  }
}

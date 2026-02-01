using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
  public GameObject player1 = null;
  public GameObject player2 = null;
  public Transform spawnPoint1, spawnPoint2;

  public CameraTarget cameraTarget;
  
  public void OnPlayerJoined()
  {
    Debug.Log("Player joined the game.");
    if (player1 == null)
    {
      Debug.Log("Assigning Player 1");
      // Find all players in the scene
      if (player1 == null)
      {
        player1 = GameObject.FindGameObjectsWithTag("Player")[0];
        player1.transform.position = spawnPoint1.position;
        Debug.Log("Player found: " + player1.name);
        cameraTarget.player1 = player1;
      }
      else
      {
        if (player2 == null)
        {
          Debug.Log("Assigning Player 2");
          player2 = GameObject.FindGameObjectsWithTag("Player")[1];
          Debug.Log("Player found: " + player2.name);
          player2.transform.position = spawnPoint2.position;
          cameraTarget.player2 = player2;
        }
      }
    }
  }
  public void OnPlayerLeft()
  {
    Debug.Log("Player left the game.");
  }

}

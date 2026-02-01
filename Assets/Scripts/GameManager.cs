using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
  public Transform spawnPoint1, spawnPoint2;

  [Header("Set up players for camera targeting")]
  public GameObject player1 = null;
  public GameObject player2 = null;
  public CameraTarget cameraTarget;
  [Header("Mask container for parried masks")]
  public Transform pippoMask;

  public void OnPlayerJoined()
  {
    Debug.Log("New Player joined the game.");

    // Find all players in the scene
    if (player1 == null)
    {
      Debug.Log("Assigning Player 1");
      player1 = GameObject.FindGameObjectsWithTag("Player")[0];
      // Move at spawn point
      player1.transform.position = spawnPoint1.position;
      // Set tag for blastzone script
      player1.tag = "Player1";
      // Set camera target
      cameraTarget.player1 = player1;
      // // Set mask container for child mask
      // var masks = FindChildGameObjectsWithTag(player1.transform, "Mask");
      // Debug.Log("masks of player " + masks.Count);
      // if (masks.Count > 0) {
      //   Debug.Log("The terrible mask container is " + pippoMask.gameObject);
      //   var component = masks[0].GetComponent<MaskScript>();
      //   Debug.Log("MaskScript component: " + component);
      //   component.masksContainer = pippoMask.gameObject;
      //   Debug.Log("Assigned mask container "+component.masksContainer+ " while manager has "+pippoMask.gameObject);
      // }
    }
    else
    {
      if (player2 == null)
      {
        Debug.Log("Assigning Player 2");
        var players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(players.Length);
        player2 = players[0];
        // Move at spawn point
        player2.transform.position = spawnPoint2.position;
        // Set tag for blastzone script
        player2.tag = "Player2";
        // Set camera target
        cameraTarget.player2 = player2;
        // // Set mask container for child mask
        // var masks = FindChildGameObjectsWithTag(player2.transform, "Mask");
        // if (masks.Count > 0)
        //   masks[0].GetComponent<MaskScript>().masksContainer = pippoMask.gameObject;
      }
    }
  }
  public void OnPlayerLeft()
  {
    Debug.Log("Player left the game.");
  }

  private List<GameObject> FindChildGameObjectsWithTag(Transform parent, string tag)
  {
    List<GameObject> taggedChildren = new List<GameObject>();
    for (int i = 0; i < parent.childCount; i++)
    {
      Transform child = parent.GetChild(i);
      if (child.CompareTag(tag))
        taggedChildren.Add(child.gameObject);
    }
    return taggedChildren;
  }
}

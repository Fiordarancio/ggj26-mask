using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

  // Update is called once per frame
  void Update()
  {

  }

    public void Select() {
      Debug.Log("UI element selected");
    }
    public void MoveSelection() {
      Debug.Log("UI element moved");
    }
}

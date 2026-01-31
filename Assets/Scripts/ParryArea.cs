using UnityEngine;

public class ParryArea : MonoBehaviour
{
    public bool canParry;
    public GameObject curMask = null;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MaskScript>() && other.GetComponent<MaskScript>().launched)
        {
            curMask = other.gameObject;
            canParry=true;
        }
    }

     void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<MaskScript>() && other.GetComponent<MaskScript>().launched)
        {
            curMask = null;
            canParry=false;
        }
    }

    public void Reset()
    {
        curMask = null;
        canParry = false;
    }
}

using UnityEngine;

public class ParryArea : MonoBehaviour
{
    public bool canParry;
    public GameObject curMask = null;

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MaskScript>())
        {
            curMask = other.gameObject;
            canParry=true;
        }
    }

     void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<MaskScript>())
        {
            curMask = null;
            canParry=false;
        }
    }


}

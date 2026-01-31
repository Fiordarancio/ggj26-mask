using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MaskScript : MonoBehaviour
{
    [SerializeField]
    public float launch_vel = 0.1f;
    [SerializeField]
    public float rot_vel = 10f;
    [SerializeField]
    public float max_dist = 10f;
    [SerializeField]
    public Vector3 defaultPosMask;


    private Vector3 facing_dir;
    public bool launched = false;
    public bool returningMask = false;
    private Vector3 launch_dir;
    private Vector3 rot_dir = new Vector3(0,0,1);

    private Vector3 startPos;

    public GameObject owner; 

    public bool CanLaunch()
    {
        return !launched;
    }

    public void LaunchMask()
    {
        launched = true;
        launch_dir = facing_dir;
        startPos = transform.position;
    }

    public void CatchMask(GameObject newParent)
    {
        if (!GameObject.ReferenceEquals(newParent, transform.parent.gameObject))
        {
            transform.SetParent(newParent.transform, true);
            owner = newParent;
        }

        launched = false;
        returningMask = false;

        if (-90 <= newParent.transform.rotation.y && newParent.transform.rotation.y <= 90)
        {
            defaultPosMask.x *= -1;
        }
        
        transform.position = newParent.transform.position + defaultPosMask;
        Vector3 curRot = transform.rotation.eulerAngles;
        transform.Rotate(0, 0, -1*curRot.z);
    }

    public void ReturnMask()
    {
        returningMask = true;
    }

    void Start()
    {
        transform.SetParent(owner.transform);
        defaultPosMask = transform.position - owner.transform.position;
    }

    void FixedUpdate()
    {
        float x_diff = transform.position.x - transform.parent.position.x;
        facing_dir = new Vector3(System.Math.Sign(x_diff), 0, 0);

        if (launched)
        {
            if (Vector3.Distance(transform.position, startPos) < max_dist && !returningMask) {
                transform.position += launch_dir * launch_vel;
                transform.Rotate(rot_dir * rot_vel);
            } else
            {
                ReturnMask();
                transform.position -= launch_dir * launch_vel;
                transform.Rotate(-rot_dir * rot_vel);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            // Player that owns mask
            if (GameObject.ReferenceEquals(collider.gameObject, owner))
            {
                CatchMask(collider.gameObject);
                Debug.Log("My MASK");
            } else
            {
                ReturnMask();
                Debug.Log("Ouch!");
            }
        }
    }
}

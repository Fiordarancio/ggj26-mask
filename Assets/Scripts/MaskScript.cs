using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MaskScript : MonoBehaviour
{
    [SerializeField]
    public float launch_vel = 0.1f;
    [SerializeField]
    public float rot_vel = 0.05f;
    [SerializeField]
    public float max_dist = 10f;
    [SerializeField]
    public Vector3 defaultPosMask;


    private Vector3 facing_dir;
    public bool launched = false;
    public bool returningMask = false;
    public Vector3 launch_dir;
    public Vector3 rot_dir = new Vector3(0,0,1);

    Vector3 startPos;

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

    public void CatchMask()
    {
        launched = false;
        returningMask = false;
        transform.Rotate(-1*transform.rotation.eulerAngles);

        if (System.Math.Sign(defaultPosMask.x) != System.Math.Sign(defaultPosMask.x))
        {
            defaultPosMask.x *= -1;
        }
        transform.position = transform.parent.position + defaultPosMask;
    }

    public void ReturnMask()
    {
        returningMask = true;
    }

    void Start()
    {
        defaultPosMask = transform.position - transform.parent.position;
    }

    void FixedUpdate()
    {
        float x_diff = transform.position.x - transform.parent.position.x;
        facing_dir = new Vector3(System.Math.Sign(x_diff), 0, 0);

        if (launched)
        {
            if (Vector3.Distance(transform.position, startPos) < max_dist && !returningMask) {
                transform.position += launch_dir * launch_vel;
                //transform.Rotate(rot_dir * rot_vel);
            } else
            {
                ReturnMask();
                transform.position -= launch_dir * launch_vel;
                //transform.Rotate(-rot_dir * rot_vel);
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
            if (GameObject.ReferenceEquals(collider.gameObject, transform.parent.gameObject))
            {
                CatchMask();
                Debug.Log("My MASK");
            } else
            {
                ReturnMask();
                Debug.Log("Ouch!");
            }
        }
    }
}

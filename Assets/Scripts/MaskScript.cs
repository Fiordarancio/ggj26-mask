using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MaskScript : MonoBehaviour
{
    [SerializeField]
    public float launch_vel = 0.1f;
    [SerializeField]
    public float rot_vel = 0.1f;
    public float max_dist = 10f;

    public bool launched = false;
    public Vector3 launch_dir;
    public Vector3 rot_dir = new Vector3(0,0,1);
    Rigidbody2D m_Rigidbody;

    public void LaunchMask(Vector3 dir)
    {
        launched = true;
        launch_dir = dir;
    }

    public bool CanLaunch()
    {
        return !launched;
    }

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (launched)
        {
            transform.position += launch_dir * launch_vel;
            transform.Rotate(transform.rotation.eulerAngles + rot_dir * rot_vel);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.IO;
using UnityEngine;

public class animateStateController : MonoBehaviour
{
    Animator animator;
    float velocity = 0.0f;
    public float acceleration = 0.1f;
    public float deceleration = 0.5f;
    int VelocityHash;


    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("velocity");

    }

    // Update is called once per frame
    void Update()
    {

        bool forwardKey = Input.GetKey("w");
        //bool runKey = Input.GetKey("left shift");

        if (forwardKey && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }else  if (!forwardKey && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }else if (!forwardKey && velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        animator.SetFloat(VelocityHash, velocity);
    }
}

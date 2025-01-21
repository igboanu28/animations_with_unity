using UnityEngine;

public class twoDAnimateStateController : MonoBehaviour
{
    Animator animator;
    float velocityX = 0.0f;
    float velocityZ = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float maximumWalkVelocity = 0.05f;
    public float maximumRunVelocity = 2.0f;
    public float previosMaxVelocity = 2.0f;
    public float smoothTime = 2.0f;
    public float maxVelocityHolder = 1f;
    public float currentMaxVelocity;
    // increase preformance
    int velocityZHash;
    int velocityXHash;

    void Start()
    {
        // get the animator component
        animator = GetComponent<Animator>();
        //increase performance
        velocityZHash = Animator.StringToHash("velocity z");
        velocityXHash = Animator.StringToHash("velocity x");
    }

    void changeVelocity(bool forwardKey, bool runKey, bool leftKey, bool rightKey, float currentMaxVelocity)
    {
        // set velocityZ and velocityX based on key inputs

        if (forwardKey)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        else
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        // Ensure that the velocityZ stays within the bounds
        velocityZ = Mathf.Clamp(velocityZ, 0.0f, currentMaxVelocity);

        if (leftKey)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        else if (rightKey)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        else
        {
            var tempVelocity = Mathf.Abs(velocityX);
            if (tempVelocity > 0.05f)
            {
                velocityX += Time.deltaTime * (velocityX > 0 ? -deceleration : acceleration);
            }
            else
            {
                velocityX = 0;
            }
        }

        // Ensure that velocityX stays within the bounds
        velocityX = Mathf.Clamp(velocityX, -currentMaxVelocity, currentMaxVelocity);
    }


    void lockOrResetVelocity(bool forwardKey, bool runKey, bool leftKey, bool rightKey, float currentMaxVelocity)
    {
        // reset velocityZ if it's below zero when no forward key is pressed
        if (!forwardKey && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }


        // reset velocityX when neither left nor right is pressed and it's close to zero
        if (!leftKey && !rightKey && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        // Lock forward and left/right movement if runKey is held and velocity exceeds max limits
        if (forwardKey && runKey && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        if (leftKey && runKey && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        if (rightKey && runKey && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get key inputs from player or user
        bool forwardKey = Input.GetKey(KeyCode.W);
        bool runKey = Input.GetKey(KeyCode.LeftShift);
        bool leftKey = Input.GetKey(KeyCode.A);
        bool rightKey = Input.GetKey(KeyCode.D);

        // Smoothly transition the max velocity between walk and run using Mathf.Lerp
        float targetMaxVelocity = runKey ? maximumRunVelocity : maximumWalkVelocity;
        currentMaxVelocity = Mathf.Lerp(currentMaxVelocity, targetMaxVelocity, smoothTime * Time.deltaTime);
        // handle velocity changes
        lockOrResetVelocity(forwardKey, runKey, leftKey, rightKey, currentMaxVelocity);
        changeVelocity(forwardKey, runKey, leftKey, rightKey, currentMaxVelocity);

        // set the animator parameters to my local variables values 
        animator.SetFloat(velocityZHash, velocityZ);
        animator.SetFloat(velocityXHash, velocityX);

    }
}

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

        if (forwardKey && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        if (leftKey && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (rightKey && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        //decelerate velocityZ to 0
        if (!forwardKey && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        // increase velocityX if left is not pressed and velocityX is less than 0
        if (!leftKey && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        if (!rightKey && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }


    void lockOrResetVelocity(bool forwardKey, bool runKey, bool leftKey, bool rightKey, float currentMaxVelocity)
    {
        //reset velocityZ
        if (!forwardKey && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }


        // rest velocityX
        if (!leftKey && !rightKey && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        // lock forward 
        if (forwardKey && runKey && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        // decelerate to the max walk velocity
        else if (forwardKey && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;

            // round to the current max velocity if within offset
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05))
            {
                velocityZ = currentMaxVelocity;
            }
        }

        // round to the current max velocity if within offset
        else if (forwardKey && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.5f))
        {
            velocityZ = currentMaxVelocity;
        }

        // lock left
        if (leftKey && runKey && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        // decelerate to the max walk velocity
        else if (leftKey && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;

            // round to the current max velocity if within offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }

        // round to the current max velocity if within offset
        else if (leftKey && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

        // lock right
        if (rightKey && runKey && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        // decelerate to the max walk velocity
        else if (rightKey && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;

            // round to the current max velocity if within offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05))
            {
                velocityX = currentMaxVelocity;
            }
        }

        // round to the current max velocity if within offset
        else if (rightKey && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
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

        // set current maxVelocity this is called a ternary operator
        float currentMaxVelocity = runKey ? maximumRunVelocity : maximumWalkVelocity;

        // handle velocity changes
        lockOrResetVelocity(forwardKey, runKey, leftKey, rightKey, currentMaxVelocity);
        changeVelocity(forwardKey, runKey, leftKey, rightKey, currentMaxVelocity);



        // set the animator parameters to my local variables values 
        animator.SetFloat(velocityZHash, velocityZ);
        animator.SetFloat(velocityXHash, velocityX);
        
    }
}

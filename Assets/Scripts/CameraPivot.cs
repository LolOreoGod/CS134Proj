using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPivot : MonoBehaviour
{
    /*
     * Responsibility: First person camera movement during special actions such as CROUCHING
     * Allows inspector to:
     *      standingHeight : Allows script to reset camera when uncrouching
     *      crouchHeight : Allows script to reset camera when crouching
     *      crouchSpeed : Allows script to match animation speed when crouching
     *      crouchForward: Allows script to match forward lean when crouching
     *      
     *      animator : Allows script to check when animations are playing
     *      crouchSmoothTime : Smoothing time when crouching
     *      standUpSmoothTime : Smoothing time when uncrouching
     *      velocityY : Tracks how fast you are currently moving in Y direction
     *      velocityZ : Tracks how fast you are currently moving in Z direction
     *      smoothTimeY : How long it should take to go from current height to target height
     *      smoothTimeZ : How long it should take to go from current z to target z
     *      
     *      
     * Why don't we just directly attach to the head bone?
     *      In first person camera, attaching to the headbone causes severe motion sickness when character is in
     *      any animation motion. Every jitter is felt in the camera + "head bobbing" effect when unintended.
     */
    public float standingHeight = 1.8f;
    public float crouchHeight = 1.0f;
    public float crouchSpeed = 10f;
    public float crouchForward = 0.5f;
    public float standingForward = 0.2f;

    public Animator animator;
    public float crouchSmoothTime = 0.1f;
    public float standUpSmoothTime = 0.3f; 
    private float velocityY;
    private float velocityZ;
    private float smoothTimeY;
    private float smoothTimeZ;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
        AdjustHeight();
    }

    /*
     * 
     * AdjustHeight() 
     * Adjusts the height and forward/backward lean of the camera when character crouches and uncrouches
     * 
     */

    void AdjustHeight()
    {
        bool isCrouching = animator.GetBool("Crouching"); //check if char is crouching via animator

        //Debug.Log("crouching?" + animator.GetBool("Crouching"));
        bool isTakedown = animator.GetBool("Takedown");
        float targetZ;
        float targetHeight;
        if (isTakedown)
        {
            animator.SetBool("Crouching", false);
            isCrouching = false;
        }

        
        if (isCrouching)
        {
            targetHeight = crouchHeight; 
            smoothTimeY = crouchSmoothTime;
        }
        else
        {
            targetHeight = standingHeight;
            smoothTimeY = standUpSmoothTime;
        }

        
        if (isCrouching)
        {
            targetZ = crouchForward;
            smoothTimeZ = crouchSmoothTime;
        }
        else
        {
            targetZ = standingForward;
            smoothTimeZ = standUpSmoothTime;
        }

        Vector3 pos = transform.localPosition;
        //smoothdamp allows for motion to ease in place. starts moving, speeds up, slows down near target
        //move pos y to targetheight, storing current velocity, over this much time
        pos.y = Mathf.SmoothDamp(pos.y, targetHeight, ref velocityY, smoothTimeY);
        pos.z = Mathf.SmoothDamp(pos.z, targetZ, ref velocityZ, smoothTimeZ);

        transform.localPosition = pos;
    }
}

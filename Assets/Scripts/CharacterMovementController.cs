using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


    /*
     * Responsibility: Controls character movement
     * Attached to: Player empty object
     * 
     * 
     * Allows inspector to:
     *      speed : Control player movement speed
     *      gravity : Control player gravity.
     *      
     *      moveAction : InputAction for moving WASD
     *      crouchAction : InputAction for crouching CTRL
     *      runAction : InputAction for run SHIFT
     *      animator : Get/set animator information
     *      
     *      walkSound : AudioSource for footsteps
     *      runSound : AudioSource for footsteps
     *      crouchWalkSound : AudioSource for footsteps
     *      
     *      controller : Get/set physics controller information
     *      velocity : Used for applying gravity and moving player
     */



public class CharacterMovementController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.8f;
    
    public InputActionReference moveAction;
    public InputActionReference crouchAction;
    public InputActionReference runAction;
    public Animator animator;

    public AudioSource walkSound;
    public AudioSource runSound;
    public AudioSource crouchWalkSound;
    

    private CharacterController controller;
    private Vector3 velocity;
    public bool isCrouching;
    public bool isMovementLocked;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    void OnEnable()
    {
        moveAction.action.Enable();
        crouchAction.action.Enable();
        runAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        crouchAction.action.Disable();
        runAction.action.Disable();
    }



    // Update is called once per frame
    void Update()
    {
        if(isMovementLocked)
        {
            if(animator != null)
            {
                animator.SetFloat("Speed", 0f);
            }
            return;
        }

        // Input
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // Movement relative to player orientation
        Vector3 move = transform.right * input.x + transform.forward * input.y;


        // Crouch Input
        isCrouching = crouchAction.action.IsPressed();

        

        // Run input
        bool isRunning = runAction.action.IsPressed();

        // Change speed based on Running/Crouching : Hardcoded for simplicity
        if (isCrouching) {
            speed = 2.75f;
        } else if (isRunning) {
            speed = 6.5f;
        } else {
            speed = 5f;
        }


        controller.Move(move * speed * Time.deltaTime);

        // Animator logic
        if (animator != null)
        {
            Vector3 horizontalVelocity = controller.velocity;
            horizontalVelocity.y = 0f;

            float speedValue = horizontalVelocity.magnitude;

            animator.SetFloat("Speed", speedValue);
            animator.SetBool("Crouching", isCrouching);
            animator.SetBool("Running", isRunning);
        }

        // Gives crouching priority over running
        if (isRunning && isCrouching) {
            isRunning = false;
        }

        // Keeps player grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        // When player is in the air, apply accelerating downforce
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Audio logic
        if (move.magnitude > 0) {
            if (!isRunning && !isCrouching)  {    
                crouchWalkSound.enabled = false;
                runSound.enabled = false;
                walkSound.enabled = true;
            } else if (isRunning) {
                walkSound.enabled = false;
                crouchWalkSound.enabled = false;
                runSound.enabled = true;
            } else if (isCrouching) {
                walkSound.enabled = false;
                runSound.enabled = false;
                crouchWalkSound.enabled = true;
            }
        } else {
            walkSound.enabled = false;
            runSound.enabled = false;
            crouchWalkSound.enabled = false;
        }
    }

    
}

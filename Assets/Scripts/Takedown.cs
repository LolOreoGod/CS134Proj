using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Takedown : MonoBehaviour
{

    public InputActionReference takedownAction;
    public Animator animator;
    public CharacterMovementController movementController;
    private CharacterController controller;
    public float takedownRange = 2f;
    public bool isPerformingTakedown;
    public float behindAngleThreshold = -0.5f;
    public Transform takedownAnchor;
    public float snapDistanceBehindEnemy = 0.6f;
    private EnemyController target;
    public CameraMouse mouseController;
    public CameraPivot cameraPivot;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }


    void OnEnable()
    {
        takedownAction.action.Enable();
        takedownAction.action.performed += OnTakedown;

    }

    void OnDisable()
    {
        takedownAction.action.performed -= OnTakedown;
        takedownAction.action.Disable();
        
    }


    // Update is called once per frame
    void Update()
    {
        // Takedown Input
        bool isTakedown = takedownAction.action.IsPressed();

        if(animator != null)
        {
            //animator.SetBool("Takedown", isTakedown);
        }
    }


    void OnTakedown(InputAction.CallbackContext context)
    {
        // if already in takedown, or if not crouching, or if no target, return
        if(isPerformingTakedown || !movementController.isCrouching)
        {
            return;
        }

        EnemyController target = validateTarget();
        if (target == null)
        {
            return;
        }

        StartTakedown(target);
    }

    EnemyController validateTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, takedownRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);
        EnemyController nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            EnemyController enemy = hit.GetComponentInParent<EnemyController>();
            if (enemy == null)
                continue;

            if (!isBehindEnemy(enemy.transform))
                continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < nearestDistance)
            {
                nearestDistance = dist;
                nearestTarget = enemy;
            }
        }

        return nearestTarget;
    }


    bool isBehindEnemy(Transform enemyTransform)
    {

        Vector3 toPlayer = (transform.position - enemyTransform.position).normalized;
        float distance = Vector3.Dot(enemyTransform.forward, toPlayer);

        return distance < behindAngleThreshold;
    }
    

    void StartTakedown(EnemyController enemy)
    {
        Debug.Log("StartTakedown called");
        isPerformingTakedown = true;
        movementController.isMovementLocked = true;
        mouseController.isLookLocked = true;
        
        target = enemy;
        animator.SetBool("Crouching", false);
        animator.SetBool("Takedown", true);
        
        
        // Snap player behind enemy - not sure if works
        Vector3 enemyForwardFlat = enemy.transform.forward;
        enemyForwardFlat.y = 0f;
        enemyForwardFlat.Normalize();

        Vector3 targetPlayerPos = enemy.transform.position - enemyForwardFlat * snapDistanceBehindEnemy;
        transform.position = targetPlayerPos;

        // Make player face same direction as enemy - works
        Vector3 faceDir = enemyForwardFlat;
        
        if (faceDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(faceDir);
        }
        
        if(animator != null)
        {
            
        }
        // Tell enemy to enter takedown as well
        enemy.StartTakedown(takedownAnchor);
    }


    // Call this from an animation event at the moment the guard should die
    public void KillCurrentTarget()
    {
        if (target == null)
            return;

        target.Die();
    }

    // Call this from an animation event at the end of the takedown animation
    public void EndTakedown()
    {
        Debug.Log("EndTakedown called");

        //tell enemy to enter endtakedown
        target.EndTakedown();

        animator.SetBool("Takedown", false);
        movementController.isMovementLocked = false;
        mouseController.isLookLocked = false;
        target = null;
        isPerformingTakedown = false;
        
    }
}

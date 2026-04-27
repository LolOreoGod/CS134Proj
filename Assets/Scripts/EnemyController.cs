using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;

public class EnemyController : MonoBehaviour
{

    private NavMeshAgent agent;
    public GameObject head;
    public Transform headBone;
    private EnemyDetectionController detection;
    public Animator animator;
    public float speed = 1f;

    // Audio Sources
    public AudioSource walkSound;
    public AudioSource investigateSound;
    public AudioSource detectSound;
    public AudioSource normalMusic;
    public AudioSource chaseMusic;
    private static int enemiesChasing = 0;
    private bool thisEnemyIsChasing = false;

    /**
     * PATROL STATE
     */
    public Transform[] patrolPoints;
    private int currentPoint = 0;
    public float minWaitTime = 10f;
    public float maxWaitTime = 20f;

    private float waitTimer = 0f;
    private float currentWaitTime = 0f;
    private bool isWaiting = false;

    /**
     * CHASE STATE
     */
    public float maxHeadAngle = 60f;
    public float bodyTurnSpeed = 5f;
    public float chaseSpeed = 2f;

    /**
     * INVESTIGATE STATE
     */
    private Vector3 lastKnownPosition;
    private bool hasLastKnownPosition = false;
    public float investigateTime = 10f;
    private float investigateTimer = 0f;
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    public float stuckThreshold = 1.5f;
    public float stuckCheckTime = 2f;
    /**
     * ATTACK STATE
     */
    public float attackRange = 2.5f;
    public float attackCooldown = 1.5f;
    private float attackTimer = 0f;
    public Collider maceCollider;
    public Mace maceDamage;

    /**
     * STUNNED STATE
     */
    public float stunMultiplier = 1f;
    private float stunTimer = 0f;

    /**
     * DEATH STATE
     */
    bool isDead = false;
    bool isDying = false;


    //STATE LOCK
    private enum State { Patrol, Chase, Investigate, Stunned}
    private State currentState;

    


    // Start is called before the first frame update
    void Start()
    {
        
        head = GetComponentInChildren<EnemyDetectionController>().gameObject;
        agent = GetComponent<NavMeshAgent>();
        
        detection = head.GetComponent<EnemyDetectionController>();

        agent.speed = speed;
        currentState = State.Patrol;
        attackTimer = attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isDead) return;
        if (isDying) return;

        if (!agent.isStopped)
        {
            walkSound.enabled = true;
        }
        else
        {
            walkSound.enabled = false;
        }

        UpdateStateMachine();
        ExecuteState();
        UpdateAnimator();
            
       // Debug.Log("Current state:" + currentState.ToString());

    }

    void SetState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        agent.isStopped = false;
        agent.ResetPath();
       
        if (newState == State.Investigate)
        {
            investigateTimer = 0f;
            lastPosition = transform.position;
            stuckTimer = 0f;
            agent.updateRotation = false;
            animator.SetBool("isInvestigating", true);
            //investigateSound.Play();
        }
        if (newState == State.Patrol) {
            agent.updateRotation = true;
            isWaiting = false;
            animator.SetBool("isInvestigating", false);
        }
    }

    void UpdateStateMachine()
    {
        if (currentState == State.Stunned)
            return;

        float distance = Vector3.Distance(transform.position, detection.player.transform.position);

        if (detection.detected)
        {
            detectSound.Play();
            SetState(State.Chase);
            detection.isAttacking = false;

            if (distance <= attackRange)
                Attack();
            return;
        }


        if (hasLastKnownPosition)
        {
            if (agent.pathPending || agent.remainingDistance > 0.5f)
            {
                SetState(State.Chase);
                detection.isAttacking = false;
            }
                
            else
            {
                SetState(State.Investigate);
                detection.isAttacking = false;
            }
                

            return;
        }

        SetState(State.Patrol);
    }

    void ExecuteState()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                ChasePlayer();
                break;

            case State.Investigate:
                Investigate();
                break;

            case State.Stunned:
                Stunned();
                break;
        }
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        Vector3 v = agent.velocity;
        v.y = 0;

        animator.SetFloat("Speed", v.magnitude);
    }

    void ChasePlayer()
    {
        StartChaseMusic();

        agent.speed = chaseSpeed;
        agent.isStopped = false;
        if (detection.player != null && detection.detected)
        {
            lastKnownPosition = detection.lastSeenPosition;
            hasLastKnownPosition = true;
            LookAtPlayer();
        }
        agent.SetDestination(lastKnownPosition);
    }


    void Patrol()
    {
        StopChaseMusic();
        agent.speed = speed;
        //check if we have patrol points
        if (patrolPoints.Length == 0) return;

        if (isWaiting)
        {
            agent.isStopped = true;
            waitTimer += Time.deltaTime;

            if (waitTimer >= currentWaitTime)
            {
                isWaiting = false;
                waitTimer = 0f;

                // move to next point
                
                currentPoint = (currentPoint + 1) % patrolPoints.Length;
               // Debug.Log("Current point: " + currentPoint);
                agent.isStopped = false;
            }

            return;
        }
        agent.isStopped = false;
        agent.SetDestination(patrolPoints[currentPoint].position);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
            isWaiting = true;
            waitTimer = 0f;
        }



    }

    void StartChaseMusic()
    {
        if (thisEnemyIsChasing) return;

        thisEnemyIsChasing = true;
        enemiesChasing++;

        if (chaseMusic != null && !chaseMusic.isPlaying)
            chaseMusic.Play();

        if (normalMusic != null && normalMusic.isPlaying)
            normalMusic.Stop();
    }

    void StopChaseMusic()
    {
        if (!thisEnemyIsChasing) return;

        thisEnemyIsChasing = false;
        enemiesChasing = Mathf.Max(0, enemiesChasing - 1);

        if (enemiesChasing == 0)
        {
            if (chaseMusic != null && chaseMusic.isPlaying)
                chaseMusic.Stop();

            if (normalMusic != null && !normalMusic.isPlaying)
                normalMusic.Play();
        }
    }

    void Investigate()
    {
        agent.speed = chaseSpeed;
        //Debug.Log("started investigation");
        agent.isStopped = false;
        agent.SetDestination(lastKnownPosition);
        //check if we've reached the last known position
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            investigateTimer += Time.deltaTime;

            if (investigateTimer >= investigateTime)
            {
                hasLastKnownPosition = false;
                SetState(State.Patrol);
                investigateTimer = 0f;
            }
            return;
        }
        if (Vector3.Distance(transform.position, lastPosition) < stuckThreshold)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= stuckCheckTime)
            {
                // We are stuck
                hasLastKnownPosition = false;
                SetState(State.Patrol);
            }
        }
        else
        {
            // We are moving
            stuckTimer = 0f;
            lastPosition = transform.position;
        }



    }

    void LookAtPlayer()
    {
        if (detection.player == null) return;

        Vector3 direction = detection.player.transform.position - transform.position;
        direction.y = 0f; // keep rotation flat

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * bodyTurnSpeed
        );
    }


    void Attack()
    {
        if (detection == null || detection.player == null) return;
        if (!detection.detected) return;

        detection.isAttacking = true;

        agent.isStopped = false;
        float distance = Vector3.Distance(transform.position, detection.player.transform.position);
        if (distance > attackRange) return;


        // slow movement but still pushing toward player
        agent.speed = chaseSpeed * 0.5f;

        // attacks at a point in direction of player, but slightly in front of, so swing doesnt go past player
        Vector3 dir = (detection.player.transform.position - transform.position).normalized;
        Vector3 attackTarget = detection.player.transform.position - dir * 1.5f;
        agent.SetDestination(attackTarget);

        LookAtPlayer();

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            animator.SetTrigger("Attack");
            //Debug.Log("Melee swung");
            
        }
    }


    public void EnableMace()
    {
        maceCollider.enabled = true;
    }

    public void DisableMace()
    {
        maceCollider.enabled = false;
    }

    void Stunned()
    {
        stunTimer -= Time.deltaTime;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        if (stunTimer <= 0f)
        {
            agent.isStopped = false;
            agent.updateRotation = true;
            agent.ResetPath();

            if (detection.player != null && detection.detected)
                SetState(State.Chase);
            else if (hasLastKnownPosition)
            {
                SetState(State.Investigate);
            }
            else
                SetState(State.Patrol);
        }
    }

    public void Stun(float duration)
    {
        if (isDead) return;
        stunTimer = duration * stunMultiplier;

        agent.isStopped = true;
        animator.SetTrigger("isStunned");
        SetState(State.Stunned);
    }
    

    public void StartTakedown(Transform anchor)
    {
        if (isDying)
        {
            return;
        }
        isDying = true;


        //STOP ALL MOVEMENT
        animator.SetFloat("Speed", 0);
        agent.isStopped = true;
        agent.ResetPath();
        agent.enabled = false;

        // Stop detection / AI
        if (detection != null)
        {
            detection.enabled = false;
        }

        //ATTACH TO PLAYER
        transform.SetParent(anchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

    }

    public void EndTakedown()
    {
        animator.SetTrigger("Death");
        transform.SetParent(null);

        isDying = false;
        Die();
    }


    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        StopChaseMusic();
        Destroy(gameObject, 3f);
    }

    


}

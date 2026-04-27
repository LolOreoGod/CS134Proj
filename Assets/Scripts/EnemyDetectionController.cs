using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetectionController : MonoBehaviour
{

    public float detectionRange = 10f;
    public float detectionAngle = 45f;

    public GameObject player;
    public bool isInAngle, isInRange, isNotHidden, detected;
    public Vector3 lastSeenPosition;
    private float loseSightBuffer = 0f;
    public float loseSightDelay = 0.5f;
    public bool isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isInAngle = false;
        isInRange = false;
        isNotHidden = false;


        Vector3 origin = transform.position;
        Vector3 toPlayer = player.transform.position - origin;
        float distanceToPlayer = toPlayer.magnitude;
        Vector3 directionToPlayer = toPlayer.normalized;

        //if guard is inside of smoke no need to do any other check.
        if (IsInsideSmoke(transform.position))
        {
            detected = false;
            lastSeenPosition = transform.position;
            return; 
        }


        if (distanceToPlayer < detectionRange)
        {
            isInRange = true;
            //Debug.Log("In range");
        }
        else
        {
            //not in range
        }

        RaycastHit[] hits = Physics.RaycastAll(
            origin,
            directionToPlayer,
            distanceToPlayer,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide
        );
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit h in hits)
        {
            // Ignore guard's own body / weapon / colliders
            if (h.transform.root == transform.root)
                continue;

            Debug.DrawLine(origin, h.point, Color.green);

            if (h.transform.root.CompareTag("Player"))
            {
                isNotHidden = true;
                break;
            }

            // Anything else blocks vision (walls, smoke, etc.)
            isNotHidden = false;
            break;
        }

        /** 
        RaycastHit hit;
        
        if (Physics.Raycast(origin, directionToPlayer, out hit, distanceToPlayer, Physics.DefaultRaycastLayers,QueryTriggerInteraction.Collide))
        {
            //Debug.Log("Ray hit: " + hit.transform.name);
            Debug.DrawLine(origin, hit.point, Color.green);
            if (hit.transform.root.CompareTag("Player"))
            {
                isNotHidden = true;
                //Debug.Log("Is not hidden");
            }
            else
            {
                isNotHidden = false;
                //player is hidden behind object
            }
        }
        else
        {
            //ray hits NOTHING. player is basically hidden
            Debug.DrawRay(origin, directionToPlayer * 100f, Color.red);
        }
        **/


        Vector3 side1 = player.transform.position - transform.position;
        Vector3 side2 = transform.forward;
        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
        if (angle < detectionAngle && angle > -1 * detectionAngle)
        {
            isInAngle = true;
            //Debug.Log("Is in angle");
        }
        else
        {
            //not in angle
        }

        if (isInAngle && isInRange && isNotHidden)
        {
            detected = true;
            lastSeenPosition = player.transform.position;
            loseSightBuffer = 0f;
        }
        else
        {

            if (isAttacking)
            {
                detected = true;
                lastSeenPosition = player.transform.position;
                loseSightBuffer = 0f;
                return;
            }


            loseSightBuffer += Time.deltaTime;

            if (loseSightBuffer > loseSightDelay)
            {
                detected = false;
            }
        }
       
    }
    //Detect if a point is inside of a smoke
    bool IsInsideSmoke(Vector3 point)
    {
        Collider[] hits = Physics.OverlapSphere(point, 0.1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Smoke"))
                return true;
        }

        return false;
    }
    void OnDrawGizmos()
    {
        if (player == null) return;

        Vector3 origin = transform.position;
        Vector3 direction = (player.transform.position - transform.position);
        Vector3 toPlayer = player.transform.position - origin;
        float distance = toPlayer.magnitude;
        RaycastHit hit;

        // Perform raycast for visualization
        if (Physics.Raycast(origin, direction, out hit, distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, hit.point);
        }
        else
        {
            // Draw full ray if nothing is hit
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, direction.normalized * 10f);
        }

    }
}

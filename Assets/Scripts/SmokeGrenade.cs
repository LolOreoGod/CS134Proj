using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /*
     * Responsibility: Smoke grenade logic and spawns SmokeVFX particle system
     * Attached to: SmokeGrenade empty object
     * 
     * 
     * Allows inspector to:
     *      radius : Radius of INVISIBLE sphere around grenade. Used purely for logic
     *      duration : How long the smoke grenade OBJECT exists. Used purely for logic.
     *      stunDuration : How long guard is stunned. Passed into EnemyControler.Stun()
     *      smokeVFX : The particle system prefab. The actual visual smoke.
     */

public class SmokeGrenade : MonoBehaviour
{
    public float radius = 3f;
    public float duration = 5f;
    public float stunDuration = 2f;
    public GameObject smokeVFX;
    public GameObject smokeSFX;
    //public AudioSource smokeSound;


    // Start is called before the first frame update
    void Start()
    {
        //smokeSound.Play();
        InstantStunEffect();
        if (smokeVFX != null)
        {
            //spawns clone of smokeVFX prefab at transform.position with no rotation applied
            Instantiate(smokeSFX, transform.position, Quaternion.identity);
            Instantiate(smokeVFX, transform.position, Quaternion.identity);
            
            
        }

        Destroy(gameObject);
    }

    /*
     * InstantStunEffect()
     * 
     * Spawns an invisible sphere of colliders. Then, check if an enemy is within. 
     * If there is an enemy, call the stun function and let EnemyController handle it.
     * 
     */
    void InstantStunEffect()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        Debug.Log("TEST HITS: " + hits.Length);
        foreach (Collider hit in hits)
        {
            Debug.Log("Hit: " + hit.name);
            EnemyController enemy = hit.GetComponentInParent<EnemyController>();

            if (enemy != null)
            {
                enemy.Stun(stunDuration);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

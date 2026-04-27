using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /*
     * Responsibility: Destroys smokeVFX clone when particle system has finished playing
     * Attached to: smokeVFX prefab
     * 
     * 
     * Allows inspector to:
     *      ps : Get information about particle system
     *      
     */



public class DestroySmoke : MonoBehaviour
{
    ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        float totalTime =
            ps.main.duration + ps.main.startLifetime.constantMax;

        Destroy(gameObject, totalTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

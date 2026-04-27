using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    /*
     * Responsibility: Controls character abilities
     * Attached to: Player empty object
     * 
     * 
     * Allows inspector to:
     *      smokeAction : Receive input from input action
     *      smokeGrenadePrefab : Spawns this prefab (has its own logic script)
     */


public class CharacterAbilityController : MonoBehaviour
{
    public InputActionReference smokeAction;
    public GameObject smokeGrenadePrefab;


    //Using .performed to call our function, prevents multiple summons per press
    void OnEnable()
    {
        smokeAction.action.Enable();
        smokeAction.action.performed += OnSmoke; //when this action is performed, call the function OnSmoke
    }

    void OnDisable()
    {
        smokeAction.action.performed -= OnSmoke; //when this action is performed, STOP calling the function OnSmoke
        smokeAction.action.Disable();
    }

    /*
     * OnSmoke(CallbackContext)
     * 
     * Spawns smokeGrenadePrefab at location slightly in front of player, on the floor
     * Unity requires this function signature to bind it to the input action
     */
    void OnSmoke(InputAction.CallbackContext context)
    {
        Vector3 spawnPos = transform.position + Vector3.up * 1f + transform.forward * 1f;

        Instantiate(smokeGrenadePrefab, spawnPos, Quaternion.identity);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouse : MonoBehaviour
{
    /*
     * Responsibility: First person camera movement
     * Allows inspector to:
     *      mouseSpeed : Edit how fast camera moves 
     *      playerModel : Transform of Player empty object allowing for entire model rotation when camera moves.
     */

    public float mouseSpeed = 200f;
    public Transform playerModel;
    public bool isLookLocked;

    private float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked; //locks mouse to center of screen
        Cursor.visible = false; //prevents player from seeing cursor
    }

    // Update is called once per frame
    void Update()
    {
        if (isLookLocked)
        {
           // Debug.Log("CameraMouse locked");
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime; //X axis mouse movement and scales with mouse sensitivty. Time.deltaTime allows smoothness across different fps values
        float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime; //Y axis mousemovment ....

        // up/down
        //Rotating around x axis results in up down movemnet
        xRotation -= mouseY; //calculates change in xRotation
        xRotation = Mathf.Clamp(xRotation, -30f, 30f); //prevents user from looking too far up or down (bent neck syndrome)

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //applies xRotation

        // left/right
        playerModel.Rotate(Vector3.up * mouseX); //rotate player model around the "up axis" (y axis) by mouseX. Since camera is attached to player, this rotates camera too
    }
}

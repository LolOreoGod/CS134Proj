using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public void RotateSettings() {
        transform.Rotate(0, -90, 0);
    }
    
    public void RotateMenu() {
        transform.Rotate(0, 90, 0);
    }

    public void Update() {

    }

    
}

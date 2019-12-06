using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines an object's behavior based on input from the mouse.
/// 
/// Reference: https://www.youtube.com/watch?v=S3pjBQObC90
/// </summary>
public class RotateObj : MonoBehaviour
{
    public float rotSpeed = 20;
 
    private void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;
        
        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }
    
    

}

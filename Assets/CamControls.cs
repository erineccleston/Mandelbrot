using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    private Vector3 point;

    public float orthographicSizeMin = 0;
    public float orthographicSizeMax = 50;
    public float zoomSpeed = 1;

    private Camera myCamera;

    

    private void Start()
    {
        point = target.transform.position;
        transform.LookAt(point);

        myCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(myCamera.orthographicSize < 1)
            {
                zoomSpeed *= 2f;
            }
            myCamera.orthographicSize += zoomSpeed;
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(myCamera.orthographicSize < 1)
            {
                zoomSpeed *= 0.5f;
            }
            myCamera.orthographicSize -= zoomSpeed;
        }

        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, 
            orthographicSizeMin, orthographicSizeMax);

        //Can be used for rotating camera around objects, needs some tweaking
        //if we go that route
        /*
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotSpeed;
            float rotY = Input.GetAxis("Mouse Y") * rotSpeed;

            transform.RotateAround(point, Vector3.up, -rotX);
            transform.RotateAround(point, Vector3.right, rotY);

        }
        */
    }


}

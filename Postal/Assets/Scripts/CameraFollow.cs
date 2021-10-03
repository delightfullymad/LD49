using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.125f;
    public Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 targetPos = target.position + offset;
        //Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smoothTime*Time.deltaTime);
        //transform.position = smoothPos;

        Vector3 newTarget = (target.position + Camera.main.ScreenToWorldPoint(Input.mousePosition)) / 2;
        Vector3 tar2 = (target.position - newTarget) / 2;
        Vector3 desiredPosition = target.position - new Vector3(tar2.x / 2, tar2.y, tar2.z) + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothTime);
        transform.position = smoothedPosition;
    }
}

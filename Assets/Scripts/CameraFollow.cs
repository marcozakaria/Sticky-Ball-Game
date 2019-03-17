using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private Vector3 lookAtOffset;

    void Start()
    {
        lookAtOffset = new Vector3(0, 1.5f, 0);
    }

    void Update()
    {
        transform.LookAt(target.position + lookAtOffset);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    private Quaternion rotationY;

    void Start()
    {

    }

    void FixedUpdate()
    {
        rotationY = Quaternion.AngleAxis(1, Vector3.up);
        transform.rotation *= rotationY;
    }
}

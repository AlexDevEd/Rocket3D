using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    private Quaternion rotationZ;
    
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        rotationZ = Quaternion.AngleAxis(1, Vector3.forward);
        transform.rotation *= rotationZ;
    }
}

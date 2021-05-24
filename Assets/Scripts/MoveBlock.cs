using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class MoveBlock : MonoBehaviour
{
    [SerializeField] private Vector3 movePosition;
    [SerializeField] private float moveSpeed;
    private Vector3 startPosition;
    [SerializeField] [Range(0, 1)] private float moveProgress;

    void Start()
    {
        startPosition = transform.position;
    }

  
    void Update()
    {
       Dislocation();
    }

    void Dislocation()
    {
        moveProgress = Mathf.PingPong(Time.time * moveSpeed, 1);
        Vector3 dislocation = movePosition * moveProgress;
        transform.position = startPosition + dislocation;
    }
   
}

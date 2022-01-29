using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform patrolPoint;
    Vector2 directionVector;
    [SerializeField] float cycleTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        directionVector =  (patrolPoint.position - transform.position).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}

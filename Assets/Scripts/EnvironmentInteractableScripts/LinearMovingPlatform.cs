using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovingPlatform : MonoBehaviour
{
    public Transform patrolPoint;
    Rigidbody2D rb;
    Vector2 directionVector;
    [SerializeField] float cycleTime = 5f;
    float maxSpeed;
    float timeSinceStart;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        directionVector =  (patrolPoint.position - transform.position);
        directionVector.Normalize();
        timeSinceStart = 0;
        maxSpeed = Vector2.Distance(patrolPoint.position, transform.position) * Mathf.PI / cycleTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Cycle();
    }

    void Cycle() {
        timeSinceStart += Time.deltaTime;
        float currentSpeed = maxSpeed * Mathf.Sin(timeSinceStart * 2 * Mathf.PI / cycleTime);
        rb.velocity = directionVector * currentSpeed;
	}
}

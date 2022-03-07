using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovingPlatform : MonoBehaviour
{
    [SerializeField] Transform patrolPoint;
    [SerializeField] float cycleTime = 5f;
    Rigidbody2D rb;
    Vector2 directionVector;

    Vector3 patrolPosition;
    float maxSpeed;
    float timeSinceStart;

	private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        patrolPosition = patrolPoint.position;
    }
	void Start()
    {
        
        directionVector =  (patrolPosition - transform.position);
        directionVector.Normalize();
        timeSinceStart = 0;
        maxSpeed = Vector2.Distance(patrolPosition, transform.position) * Mathf.PI / cycleTime;
        
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

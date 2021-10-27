using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private bool isGravityActive = false;
    private float gravity = 9.8f;

    private void FixedUpdate()
    {
        if (isGravityActive)
        {
            Fall();
        }
    }

    public void ActivateGravity()
    {
        isGravityActive = true;
    }

    private void Fall()
    {
        transform.position -= new Vector3(0, gravity * Time.deltaTime, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGravityActive = false;
    }
}

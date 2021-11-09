using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGrappling : MonoBehaviour
{
    float defaultGravity;
    Rigidbody2D rb;

    CharacterController2DAlt controller;

    [Header("Grappling:")]
    public Tutorial_GrapplingGun grappleGun;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravity = rb.gravityScale;
        controller = GetComponent<CharacterController2DAlt>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grappleGun.grappleRope.isGrappling)
            rb.gravityScale = 0;
        else
            rb.gravityScale = defaultGravity;

        controller.jumpReleaseActive = !grappleGun.grappleRope.isGrappling;
    }
}

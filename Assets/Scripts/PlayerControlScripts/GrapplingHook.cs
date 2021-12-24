using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The hook used by the grappling gun, should be instantiated on a hook object
 * that is launched upon firing the grappling gun
 * 
 * note that due to low framerate the trigger detection may go through thin colliders,
 * thus the trigger should be set large enough to cover the error margin of velocity/fps units length
 */
public class GrapplingHook : MonoBehaviour
{
    public bool isAttached;
    public bool grappleRelease; //true if currently on the way back from attached point
    public bool hasReturned;
    Camera cam;
    Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sprite;
    public Transform gunPoint;

    [Range(0, 30)] public float maxRange = 10;
    
    public float velocity = 100f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        hasReturned = true;
    }

    public void Grapple()
    {
        hasReturned = false;
        sprite.enabled = true;
        transform.position = gunPoint.position;
        //upon enable launch in the direction of the mouse
        Vector2 distanceVector = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        rb.velocity = distanceVector * velocity;
    }

	private void Update() {
        if (Vector2.Distance(transform.position, gunPoint.position) > maxRange) {
            grappleRelease = true;
        }
	}

	private void OnTriggerEnter2D(Collider2D collision) {
        if (sprite.enabled && collision.gameObject.CompareTag("Grappable")) {
            rb.velocity = Vector2.zero;
            isAttached = true;
        }
    }

	private void FixedUpdate() {
		if (grappleRelease) {
            ReturnHook();
		}
	}

	void ReturnHook () {
        isAttached = false;
        Vector2 distanceVector = (gunPoint.position - transform.position).normalized;
        rb.velocity = distanceVector * velocity;

        if (Vector2.Distance(transform.position,gunPoint.position) <= 3f) {
            transform.position = gunPoint.position;
            rb.velocity = Vector2.zero;
            sprite.enabled = false;
            hasReturned = true;
            grappleRelease = false;
        }
    }
}
//add rendering of the grappling rope
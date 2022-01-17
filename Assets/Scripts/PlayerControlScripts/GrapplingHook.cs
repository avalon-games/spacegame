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
    Camera cam;
    Rigidbody2D rb;
    public Transform gunPoint;
    LineRenderer rope;

    [Range(0, 30)] public float maxRange = 10;
    
    public float launchSpeed = 100f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        rope = GetComponent<LineRenderer>();
        this.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        isAttached = false;
        transform.position = gunPoint.position;
        Vector2 distanceVector = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        distanceVector.Normalize();
        rb.velocity = distanceVector * launchSpeed;
    }

	private void LateUpdate() {
        if (!isAttached && (Vector2.Distance(transform.position, gunPoint.position) > maxRange)) {
            grappleRelease = true;
        }
        rope.SetPosition(0, gunPoint.position);
        rope.SetPosition(1, transform.position);
    }

	private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Grappable")) {
            //snap position to centre of tile, each grid is 0.99 in size, /0.99 gets actual grid cell number, *0.99 translates it back to world coordinate
            transform.position = new Vector2(Mathf.Floor(transform.position.x / 0.99f) * 0.99f + 0.495f,
                                            Mathf.Floor(transform.position.y / 0.99f) * 0.99f + 0.495f);
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
        rb.velocity = distanceVector * launchSpeed;

        if (Vector2.Distance(transform.position,gunPoint.position) <= 3f) {
            transform.position = gunPoint.position;
            rb.velocity = Vector2.zero;
            grappleRelease = false;
            this.gameObject.SetActive(false);
        }
    }
}
//add rendering of the grappling rope
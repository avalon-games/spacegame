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
    Transform hitDynamic;
    Vector2 hitStatic;
    public Sprite attachedSprite;
    Sprite detachedSprite;
    SpriteRenderer sp;

    [Range(0, 30)] public float maxRange = 10;

    Vector2 launchDirection;
    [SerializeField] float launchSpeed = 31.9f;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        rope = GetComponent<LineRenderer>();
        sp = GetComponent<SpriteRenderer>();
        detachedSprite = sp.sprite;
        this.gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        isAttached = false;
        transform.position = gunPoint.position;
        launchDirection = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        launchDirection.Normalize();
        rb.velocity = launchDirection * launchSpeed;

        GetHitObject(launchDirection);
        if (hitDynamic != null) {
            float timeToHit = Vector2.Distance(gunPoint.position, hitDynamic.position)/launchSpeed;
            StartCoroutine(AttachAfterTime(timeToHit));
        }  else if (hitStatic != Vector2.negativeInfinity) {
            float timeToHit = Vector2.Distance(gunPoint.position, hitStatic) / launchSpeed;
            StartCoroutine(AttachAfterTime(timeToHit));
        }
    }

	private void LateUpdate() {
		if (!isAttached && (Vector2.Distance(transform.position, gunPoint.position) > maxRange)) {
			grappleRelease = true;
			ClearAnchorPoint();
		} else if (isAttached) {
            PositionAtHitPoint();
		}
		SetRopeEndpoints();
		SetRotation();
        SetSprite();
	}

	private void SetRopeEndpoints() {
		rope.SetPosition(0, gunPoint.position);
		rope.SetPosition(1, transform.position);
	}

	private void SetRotation() {
		if (hitDynamic) {
			transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(hitDynamic.position.y - gunPoint.position.y, hitDynamic.position.x - gunPoint.position.x) * 180 / Mathf.PI - 45);
		} else {
			transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(launchDirection.y, launchDirection.x) * 180 / Mathf.PI - 45);
		}
	}
    void SetSprite() {
		if (isAttached) {
            sp.sprite = attachedSprite;
		}
        else {
            sp.sprite = detachedSprite;
		}
	}

	void GetHitObject(Vector2 direction) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(gunPoint.position, direction, maxRange);

        foreach(RaycastHit2D hit in hits) {
            if(hit.collider.CompareTag("Grappable") || hit.collider.GetComponent<Grappable>()) {
                //if on grappable tilemap, return a static position
                Rigidbody2D hitRb = hit.collider.GetComponent<Rigidbody2D>();
                if (hitRb == null || hitRb.bodyType == RigidbodyType2D.Static) {
                    //print(hit.point);
                    hitStatic = new Vector2(Mathf.Floor(hit.point.x / 0.99f) * 0.99f + 0.495f,
                                         Mathf.Floor(hit.point.y / 0.99f) * 0.99f + 0.495f);
                    hitDynamic = null;
                } else {
                    hitDynamic = hit.collider.transform;
                    hitStatic = Vector2.negativeInfinity;
                }
                break;
			}
		}
     }
    void ClearAnchorPoint() {
        hitDynamic = null;
        hitStatic = Vector2.negativeInfinity;
    }
    public Vector2 GetAnchorPoint() {
        if(hitDynamic) {
            return hitDynamic.position;
		} else {
            return hitStatic;
		}
    }

    void Attach() {
		PositionAtHitPoint();
        rb.velocity = Vector2.zero;

		isAttached = true;
	}

	private void PositionAtHitPoint() {
        if (hitDynamic) transform.position = hitDynamic.position;
        else if (hitStatic != Vector2.negativeInfinity) transform.position = hitStatic;
	}

	IEnumerator AttachAfterTime(float time) {
        yield return new WaitForSeconds(time);
        if (hitDynamic != null || hitStatic != Vector2.negativeInfinity) Attach();
	}

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    if (collision.gameObject.CompareTag("Grappable")) {
    //        //snap position to centre of tile, each grid is 0.99 in size, /0.99 gets actual grid cell number, *0.99 translates it back to world coordinate
    //        transform.position = new Vector2(Mathf.Floor(transform.position.x / 0.99f) * 0.99f + 0.495f,
    //                                        Mathf.Floor(transform.position.y / 0.99f) * 0.99f + 0.495f);
    //        rb.velocity = Vector2.zero;
           
    //        isAttached = true;
    //    }
    //}

	private void FixedUpdate() {
        if (grappleRelease) {
            ReturnHook();
		}
	}

	void ReturnHook () {
        isAttached = false;
        Vector2 distanceVector = (gunPoint.position - transform.position);
        distanceVector.Normalize();
        rb.velocity = distanceVector * launchSpeed;

        if (Vector2.Distance(transform.position,gunPoint.position) <= 1f) {
            transform.position = gunPoint.position;
            rb.velocity = Vector2.zero;
            grappleRelease = false;
            this.gameObject.SetActive(false);
        }
    }

	private void OnDisable() {
        transform.position = gunPoint.position;
        rb.velocity = Vector2.zero;
        grappleRelease = false;
        isAttached = false;
        ClearAnchorPoint();
	}
}
//add rendering of the grappling rope
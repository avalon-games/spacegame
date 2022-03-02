using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
	public GameObject pullHook;
	public GameObject swingHook;
	GrapplingHook pullHookScript;
	GrapplingHook swingHookScript;

	public PlayerController player;
	public PlayerMovement mover;
	float initialGravity;

	public float pullSpeed = 30f;
	public float swingSpeedMultiplier = 2f;
	public float pullReleaseMultiplier = 1 / 8f;
	public float swingReleaseMultiplier = 1.5f;
	bool swingRight; //if false, swing to the left

	bool initializeSwing;
	bool initializePull;

	//defines how many times the player can grapple
	public int totalCharge;
	public bool infiniteCharge;
	bool pullRelease;
	bool swingRelease;
	bool slowMo;
	private Vector3 refVel = Vector3.zero;



	Rigidbody2D rb;

	private void Start() {
		pullHookScript = pullHook.GetComponent<GrapplingHook>();
		swingHookScript = swingHook.GetComponent<GrapplingHook>();
		rb = player.GetComponent<Rigidbody2D>();
		mover = player.GetComponent<PlayerMovement>();
		initialGravity = rb.gravityScale;


	}

	void Update () {
		//replenish charge while on ground
		if (mover.isOnGround) {
			totalCharge = 3;
		}

		ManageInput();

		if (!pullHook.activeSelf && !swingHook.activeSelf) {
			if (pullRelease) {
				rb.velocity = pullReleaseMultiplier * rb.velocity;
				pullRelease = false;
			}
			if (swingRelease) {
				rb.velocity = swingReleaseMultiplier * rb.velocity;
				swingRelease = false;
			}
			rb.gravityScale = initialGravity;
		}

		//freeze movement during launch
		if ((!pullHookScript.isAttached && pullHook.activeSelf && !pullHookScript.grappleRelease) ||
			(!swingHookScript.isAttached && swingHook.activeSelf && !swingHookScript.grappleRelease)) {
			rb.velocity = Vector2.zero;
		}
		//while attached, translate the player
		if (pullHook.activeSelf && pullHookScript.isAttached) {
			mover.isOnGround = false;
			if (initializePull) InitializePull();
			if (Vector2.Distance(transform.position, pullHook.transform.position) >= 1f) {
				Pull();
			} else {
				//print(Vector2.Distance(transform.position, pullHook.transform.position));
				Rigidbody2D playerParentRB = player.transform.parent ? player.transform.parent.GetComponent<Rigidbody2D>() : null;
				rb.velocity = playerParentRB ? playerParentRB.velocity : Vector2.zero;
			}
		} else if (swingHook.activeSelf && swingHookScript.isAttached) {
			if (initializeSwing) InitializeSwing();
			if (!swingHookScript.grappleRelease) Swing();
		}
	}

	private void ManageInput() {
		//assign action based on button press
		if (Input.GetButtonDown("GrappleSwing") && (totalCharge > 0 || infiniteCharge)) {
			swingHook.SetActive(false);
			swingHook.SetActive(true);
			initializeSwing = true;
			totalCharge--;

			if (pullHook.activeSelf) {
				pullHookScript.grappleRelease = true;
			}

		} else if (Input.GetButtonDown("GrapplePull") &&  (totalCharge > 0 || infiniteCharge)) {
			pullHook.SetActive(false);
			pullHook.SetActive(true);
			totalCharge--;
			initializePull = true;

			if (swingHook.activeSelf) {
				swingHookScript.grappleRelease = true;
			}

			//when released, reenable movement
  		} else if (Input.GetButtonUp("GrappleSwing") && swingHook.activeSelf) {
			swingHookScript.grappleRelease = true;
			swingRelease = true;
		} else if (Input.GetButtonUp("GrapplePull") && pullHook.activeSelf) {
			pullHookScript.grappleRelease = true;
			pullRelease = true;
		}
	}

	/**
	 * Initialize pulling
	 * set gravity scale to 0 while pulling
	 */
	void InitializePull () {
		initializePull = false;
		rb.gravityScale = 0;
	}

	/**
	 * Translates the player towards the grapple point
	 */
	void Pull () {
		Vector2 direction = (pullHook.transform.position - transform.position).normalized;
		Vector3 targetVel = direction * this.pullSpeed;
		rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref refVel, 0.05f);
	}


	/**
	 * Initialize the pivot point for swinging once hook has attached and 
	 * set the direction of swinging
	 */
	private void InitializeSwing() {
		initializeSwing = false;
		float xPos = transform.position.x - swingHook.transform.position.x;
		float currentPhase = GetSwingPhase();

		if (currentPhase > Mathf.PI/6 && currentPhase < 5*Mathf.PI / 6) swingHookScript.grappleRelease = true; //don't allow swing if above 30 degrees the grappled point
		if (xPos < 0) swingRight = true; //swing based on where player is relative to grapple
		else swingRight = false;
	}

	void Swing() {
		float currentPhase = GetSwingPhase();

		if (currentPhase > (0 + 0.5f) && currentPhase < (Mathf.PI - 0.5f)) swingHookScript.grappleRelease = true;
		float xPos = transform.position.x - swingHook.transform.position.x;
		float yPos = transform.position.y - swingHook.transform.position.y;
		float length = new Vector2(xPos, yPos).magnitude;

		float potentialEnergy = rb.gravityScale * 9.81f * (length + length * Mathf.Sin(currentPhase));
		float totalEnergy = rb.gravityScale * 9.81f * (length*2f);
		float kineticEnergy = totalEnergy - potentialEnergy;
		float velocity = Mathf.Sqrt(2 * kineticEnergy);

		
		float velocityPhase;
		if (swingRight)
			velocityPhase = currentPhase + Mathf.PI / 2;
		else
			velocityPhase = currentPhase - Mathf.PI / 2;

		rb.velocity = new Vector2(Mathf.Cos(velocityPhase),
										 Mathf.Sin(velocityPhase)).normalized * velocity * swingSpeedMultiplier;
	}

	float GetSwingPhase() {
		float currentPhase;
		float xPos = transform.position.x - swingHook.transform.position.x;
		float yPos = transform.position.y - swingHook.transform.position.y;
		if (xPos > 0 && yPos > 0) currentPhase = Mathf.Atan(Mathf.Abs(yPos / xPos));
		else if (xPos < 0 && yPos > 0) currentPhase = Mathf.PI - Mathf.Atan(Mathf.Abs(yPos / xPos));
		else if (xPos < 0 && yPos < 0) currentPhase = Mathf.PI + Mathf.Atan(Mathf.Abs(yPos / xPos));
		else currentPhase = 2*Mathf.PI - Mathf.Atan(Mathf.Abs(yPos / xPos));
		return currentPhase;
	}

	public void ToggleInfiniteCharge() {
		infiniteCharge = !infiniteCharge;
	}

	public void DisableGrapple() {
		pullHook.SetActive(false);
		swingHook.SetActive(false);
	}

	public bool IsAttached() {
		return pullHookScript.isAttached || swingHookScript.isAttached;
	}

	public Vector2 GetAnchorPoint() {
		if (pullHookScript.isAttached) {
			return pullHookScript.GetAnchorPoint();
		} else if (swingHookScript.isAttached) {
			return swingHookScript.GetAnchorPoint();
		} else
			return Vector2.negativeInfinity;
	}
}

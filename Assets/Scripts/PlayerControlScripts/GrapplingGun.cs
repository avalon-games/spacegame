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
	float initialGravity;

	public float pullSpeed = 40f;
	public float swingSpeed = 20f;
	bool swingRight; //if false, swing to the left

	float currentPhase;
	bool initializeSwing;
	bool initializePull;

	//defines how many times the player can grapple
	public int pullCharge;
	public int swingCharge;
	public bool infiniteCharge;

	private void Start() {
		pullHookScript = pullHook.GetComponent<GrapplingHook>();
		swingHookScript = swingHook.GetComponent<GrapplingHook>();
		initialGravity = player.rb.gravityScale;
	}

	void Update () {
		//replenish charge while on ground
		if (player.isOnGround) {
			pullCharge = 2;
			swingCharge = 2;
		}

		ManageInput();

		if (pullHookScript.hasReturned && swingHookScript.hasReturned) {
			player.ToggleMovementControl(true);
			player.rb.gravityScale = initialGravity;
		}

		//freeze movement during launch
		if ((!pullHookScript.isAttached && !pullHookScript.hasReturned && !pullHookScript.grappleRelease) || (!swingHookScript.isAttached && !swingHookScript.hasReturned && !swingHookScript.grappleRelease))
			player.rb.velocity = Vector2.zero;

		//while attached, translate the player
		if (pullHookScript.isAttached) {
			player.isOnGround = false;
			if (initializePull) InitializePull();
			if (Vector2.Distance(transform.position, pullHook.transform.position) >= 1f) Pull();
			else player.rb.velocity = Vector2.zero;
		} else if (swingHookScript.isAttached) {
			if (initializeSwing) InitializeSwing();
			if (!swingHookScript.grappleRelease) Swing();
			//else player.rb.velocity = Vector2.right;
		}
	}

	private void ManageInput() {
		//assign action based on button press
		if (Input.GetButtonDown("GrappleSwing") && swingHookScript.hasReturned && (swingCharge > 0 || infiniteCharge)) {
			swingHookScript.Grapple();
			player.ToggleMovementControl(false);
			initializeSwing = true;
			swingCharge--;

			if (!pullHookScript.hasReturned) {
				pullHookScript.grappleRelease = true;
			}

		} else if (Input.GetButtonDown("GrapplePull") && pullHookScript.hasReturned && (pullCharge > 0 || infiniteCharge)) {
			pullHookScript.Grapple();

			if (!swingHookScript.hasReturned) {
				swingHookScript.grappleRelease = true;
			}

			player.ToggleMovementControl(false);

			pullCharge--;
			initializePull = true;

			//when released, reenable movement
		} else if (Input.GetButtonUp("GrappleSwing") || Input.GetButtonUp("GrapplePull")) {
			if (!pullHookScript.hasReturned)
				pullHookScript.grappleRelease = true;
			if (!swingHookScript.hasReturned)
				swingHookScript.grappleRelease = true;
		}
	}

	/**
	 * Initialize pulling
	 * set gravity scale to 0 while pulling
	 */
	void InitializePull () {
		initializePull = false;
		player.rb.gravityScale = 0;
	}

	/**
	 * Translates the player towards the grapple point
	 */
	void Pull () {
		Vector2 direction = (pullHook.transform.position - transform.position).normalized;
		player.rb.velocity = direction * pullSpeed;
	}


	/**
	 * Initialize the pivot point for swinging once hook has attached and 
	 * set the direction of swinging
	 */
	private void InitializeSwing() {
		initializeSwing = false;
		float xPos = transform.position.x - swingHook.transform.position.x;
		float yPos = transform.position.y - swingHook.transform.position.y;

		if (yPos > 0) swingHookScript.grappleRelease = true; //don't allow swing if above the grappled point
		if (xPos < 0) swingRight = true;
		else swingRight = false;
	}

	void Swing() {
		GetSwingPhase();

		if (currentPhase > (0 + 0.5f) && currentPhase < (Mathf.PI- 0.5f)) swingHookScript.grappleRelease = true;

		float velocityPhase;
		if (swingRight)
			velocityPhase = currentPhase + Mathf.PI / 2;
		else
			velocityPhase = currentPhase - Mathf.PI / 2;

		player.rb.velocity = new Vector2(Mathf.Cos(velocityPhase),
										 Mathf.Sin(velocityPhase)).normalized * swingSpeed;
	}

	void GetSwingPhase() {
		float xPos = transform.position.x - swingHook.transform.position.x;
		float yPos = transform.position.y - swingHook.transform.position.y;
		if (xPos > 0 && yPos > 0) currentPhase = Mathf.Atan(Mathf.Abs(yPos / xPos));
		else if (xPos < 0 && yPos > 0) currentPhase = Mathf.PI - Mathf.Atan(Mathf.Abs(yPos / xPos));
		else if (xPos < 0 && yPos < 0) currentPhase = Mathf.PI + Mathf.Atan(Mathf.Abs(yPos / xPos));
		else currentPhase = 2*Mathf.PI - Mathf.Atan(Mathf.Abs(yPos / xPos));
	}
}
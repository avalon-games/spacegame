using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
	GrapplingHook hookScript;

	public GameObject hook;
	public PlayerController player;
	bool grappleSwing;
	bool grapplePull;
	float initialGravity;

	public float pullSpeed = 40f;
	public float swingAngularFrequency = 10f;
	bool swingRight; //if false, swing to the left

	float initialPhase;
	float currentPhase;
	float swingRadius;
	bool initializeSwing;
	bool initializePull;

	private void Start() {
		hookScript = hook.GetComponent<GrapplingHook>();
		initialGravity = player.rb.gravityScale;
	}

	void Update () {
		if (Input.GetButtonDown("GrappleSwing") && hookScript.hasReturned) {
			hookScript.Grapple();
			grappleSwing = true;
			player.ToggleMovementControl(false);
			initializeSwing = true;

		} else if (Input.GetButtonDown("GrapplePull") && hookScript.hasReturned) {
			hookScript.Grapple();
			grapplePull = true;
			player.ToggleMovementControl(false);
			initializePull = true;

		} else if (Input.GetButtonUp("GrappleSwing") || Input.GetButtonUp("GrapplePull")) {
			grappleSwing = false;
			grapplePull = false;
			if (!hookScript.hasReturned)
				hookScript.grappleRelease = true;
		}

		if (hookScript.isAttached) {
			if (grapplePull) {
				if (initializePull) InitializePull();
				if (Vector2.Distance(transform.position, hook.transform.position) >= 1f) Pull();
				else player.rb.velocity = Vector2.zero;
			}
			else if (grappleSwing) {
				if (initializeSwing) InitializeSwing();
				Swing();
			}
		}
		if (hookScript.grappleRelease) {
			player.ToggleMovementControl(true);
			player.rb.gravityScale = initialGravity;
		}

	}

	/**
	 * Initialize pulling
	 * set gravity scale to 0 while pulling
	 */
	void InitializePull () {
		initializePull = false;
		player.rb.gravityScale = 0;
		hook.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
	}

	/**
	 * Translates the player towards the grapple point
	 */
	void Pull () {
		Vector2 direction = (hook.transform.position - transform.position).normalized;
		player.rb.velocity = direction * pullSpeed;

	}


	/**
	 * Initialize the pivot point for swinging once hook has attached and 
	 * set the direction of swinging, initial phase
	 */
	private void InitializeSwing() {
		initializeSwing = false;
		float xPos = transform.position.x - hook.transform.position.x;
		float yPos = transform.position.y - hook.transform.position.y;
		if (xPos < 0) swingRight = true;
		else swingRight = false;

		initialPhase = Mathf.Atan(yPos / xPos);
		currentPhase = initialPhase;
		swingRadius = Mathf.Sqrt(xPos * xPos + yPos * yPos);
		hook.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
	}

	void Swing() {
		//enable swinging movement control
		//swing

		//phase should only change until you hit a wall
		currentPhase += Time.deltaTime * swingAngularFrequency;

		if (swingRight) {
			player.transform.position = new Vector2(hook.transform.position.x + swingRadius*Mathf.Cos(currentPhase),
				hook.transform.position.y + swingRadius*Mathf.Sin(currentPhase));
		} else {
			player.transform.position = new Vector2(hook.transform.position.x + swingRadius * Mathf.Cos(currentPhase),
				hook.transform.position.y + -swingRadius*Mathf.Sin(currentPhase));
		}
	}

	//instantiate hook on button press, launch in the direction of mouse with an initial velocity, decellerating over time
	//while hook is out, disable mouse control over the gun pivot, gun pivot direction only follows the hook location
	//when hook is attached (bool), either swing or launch towards the target based on mode of operation
	//swing - use centripetal force formula to set velocity, use quaterion to rotate this velocity over time
	//pull - launches with an initial velocity that maintains constant towards the point, velocity = 0 upon reaching the point
	//release - detaches the hook and retracts it to the gun pivot
	//just use a magnetic grappling hook for now so you don't worry about the line renderer
}
//controls the firing of grappling gun



//instantiate > while it is attached,pull player towards the point
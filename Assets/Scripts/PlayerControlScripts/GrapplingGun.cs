using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
	public GameObject hook;
	GrapplingHook hookScript;
	public PlayerController player;
	bool grappleSwing;
	bool grapplePull;

	public float launchSpeed = 10f;

	private void Start() {
		hookScript = hook.GetComponent<GrapplingHook>();
	}

	void Update () {
		if (Input.GetButtonDown("GrappleSwing") && hookScript.hasReturned) {
			hookScript.Grapple();
			grappleSwing = true;
			player.ToggleMovementControl(false);

		} else if (Input.GetButtonDown("GrapplePull") && hookScript.hasReturned) {
			hookScript.Grapple();
			grapplePull = true;
			player.ToggleMovementControl(false);

		} else if (Input.GetButtonUp("GrappleSwing") || Input.GetButtonUp("GrapplePull")) {
			grappleSwing = false;
			grapplePull = false;
			if (!hookScript.hasReturned)
				hookScript.grappleRelease = true;
		}

		if (hookScript.isAttached) {
			if (grapplePull) {
				if (Vector2.Distance(transform.position, hook.transform.position) >= 1f)
					Pull();
				else
					player.rb.velocity = Vector2.zero;
			}
			else if (grappleSwing) {
				Swing();
			}
		}
		if (hookScript.grappleRelease)
			player.ToggleMovementControl(true);

	}

	/**
	 * Translates the player towards the grapple point
	 */
	void Pull () {
		Vector2 direction = (hook.transform.position - transform.position).normalized;
		player.rb.velocity = direction * launchSpeed;
	}
	void Swing() {
		Debug.Log("Swinging!");
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
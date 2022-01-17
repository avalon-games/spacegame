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
	public int totalCharge;
	public bool infiniteCharge;
	bool slowMo;

	private void Start() {
		pullHookScript = pullHook.GetComponent<GrapplingHook>();
		swingHookScript = swingHook.GetComponent<GrapplingHook>();
		initialGravity = player.rb.gravityScale;
	}

	void Update () {
		//replenish charge while on ground
		if (player.isOnGround) {
			totalCharge = 3;
		}

		ManageInput();

		if (!pullHook.activeSelf && !swingHook.activeSelf) {
			player.ToggleMovementControl(true);
			player.rb.gravityScale = initialGravity;
		}

		//freeze movement during launch
		if ((!pullHookScript.isAttached && pullHook.activeSelf && !pullHookScript.grappleRelease) ||
			(!swingHookScript.isAttached && swingHook.activeSelf && !swingHookScript.grappleRelease)) {
			player.rb.velocity = Vector2.zero;
		}
		//while attached, translate the player
		if (pullHookScript.isAttached) {
			player.isOnGround = false;
			if (initializePull) InitializePull();
			if (Vector2.Distance(transform.position, pullHook.transform.position) >= 1f) {
				StartCoroutine(SlowMo(0.1f));
				Pull();
			} else player.rb.velocity = Vector2.zero;
		} else if (swingHookScript.isAttached) {
			if (initializeSwing) InitializeSwing();
			if (!swingHookScript.grappleRelease) Swing();
		}
	}

	private void ManageInput() {
		//assign action based on button press
		if (Input.GetButtonDown("GrappleSwing") && !swingHook.activeSelf && (totalCharge > 0 || infiniteCharge)) {
			swingHook.SetActive(true);
			player.ToggleMovementControl(false);
			initializeSwing = true;
			totalCharge--;

			if (pullHook.activeSelf) {
				pullHookScript.grappleRelease = true;
			}

		} else if (Input.GetButtonDown("GrapplePull") && !pullHook.activeSelf && (totalCharge > 0 || infiniteCharge)) {
			pullHook.SetActive(true);
			player.ToggleMovementControl(false);
			totalCharge--;
			initializePull = true;

			if (swingHook.activeSelf) {
				swingHookScript.grappleRelease = true;
			}

			//when released, reenable movement
  		} else if (Input.GetButtonUp("GrappleSwing")) {
			if (swingHook.activeSelf) swingHookScript.grappleRelease = true;
		} else if (Input.GetButtonUp("GrapplePull")) {
			if (pullHook.activeSelf) pullHookScript.grappleRelease = true;
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
		GetSwingPhase();

		if (currentPhase > Mathf.PI/6 && currentPhase < 5*Mathf.PI / 6) swingHookScript.grappleRelease = true; //don't allow swing if above 30 degrees the grappled point
		if (xPos < 0) swingRight = true; //swing based on where player is relative to grapple
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

	public void ToggleInfiniteCharge() {
		infiniteCharge = !infiniteCharge;
	}

	public void DisableGrapple() {
		pullHook.SetActive(false);
		swingHook.SetActive(false);
	}

	IEnumerator SlowMo(float seconds) {
		if (!slowMo) {
			Time.timeScale = 0.5f;
			slowMo = true;
		}
		yield return new WaitForSecondsRealtime(seconds);
		Time.timeScale = 1.0f;
		slowMo = false;
	}
}

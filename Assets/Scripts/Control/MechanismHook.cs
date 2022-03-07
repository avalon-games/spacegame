using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
public class MechanismHook : MonoBehaviour
{
	Rigidbody2D rb;
	public Transform gunPoint;
	[SerializeField] float launchSpeed = 20f;
	Camera cam;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
	}

	private void Start() {
		transform.position = gunPoint.position;
		Vector2 launchDirection = (cam.ScreenToWorldPoint(Input.mousePosition) - transform.position);
		launchDirection.Normalize();
		rb.velocity = launchDirection * launchSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		PressurePlateTrigger trigger = collision.GetComponent<PressurePlateTrigger>();
		if (trigger) {
			rb.velocity = Vector2.zero;
			Destroy(this.gameObject, 2f);
		}
	}
}

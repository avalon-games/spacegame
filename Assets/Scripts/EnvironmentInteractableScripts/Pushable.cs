using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
	Collider2D coll;
	Rigidbody2D rb;
	[Range(0,20)][SerializeField] float pushSpeed = 2;

	private void Start() {
		coll = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			if (collision.collider.bounds.max.x < coll.bounds.min.x) //pushing right
				rb.velocity = new Vector2(pushSpeed, 0f);
			else if (collision.collider.bounds.min.x > coll.bounds.max.x) //pushing left
				rb.velocity = new Vector2(-pushSpeed, 0f);
			else
				rb.velocity = Vector2.zero;
		}
	}

	private void OnCollisionExit(Collision collision) {
		if (collision.gameObject.CompareTag("Player"))
			rb.velocity = Vector2.zero;
	}

}

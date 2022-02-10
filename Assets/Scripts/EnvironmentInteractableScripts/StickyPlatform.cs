using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
	[SerializeField] Transform player;
	bool isHooked;

	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.CompareTag("Player")) {
			if (PlayerIsAbovePlatform(collision.collider)) {
				player.SetParent(transform);
			}
		}
	}
	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			player.SetParent(null);
		}
	}

	private bool PlayerIsAbovePlatform(Collider2D coll) {
		float bottomPositionY = coll.bounds.center.y - coll.bounds.extents.y;
		return bottomPositionY > transform.position.y;
	}



	private void OnTriggerStay2D(Collider2D collision) {
		if(collision.gameObject.CompareTag("Hook")) {
			GrapplingHook hook = collision.GetComponent<GrapplingHook>();
			if (hook && hook.isAttached) {
				player.SetParent(transform);
				isHooked = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Hook") && isHooked) {	
			player.SetParent(null);
			isHooked = false;
		}
	}
}

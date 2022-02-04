using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.CompareTag("Player")) {
			
			if (PlayerIsAbovePlatform(collision.collider)) {
				print("Sticking");
				collision.transform.SetParent(transform.parent, true);
			}
		}
	}

	private bool PlayerIsAbovePlatform(Collider2D player) {
		float bottomPositionY = player.bounds.center.y - player.bounds.extents.y;
		return bottomPositionY > transform.position.y;
	}

	private void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Player")) {
			collision.transform.SetParent(null);
		}
	}
}

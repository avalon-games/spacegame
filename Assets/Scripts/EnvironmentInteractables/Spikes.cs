using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Spikes deal damage to the player on collision
 * - requires trigger
 */
public class Spikes : DamagingHazards
{
	private new void Start() {
		base.Start();
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if(collision.gameObject.CompareTag("Player")) {
			Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
			DealDamage();
		}
	}
}

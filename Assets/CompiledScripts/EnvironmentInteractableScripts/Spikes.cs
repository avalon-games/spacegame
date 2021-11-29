using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Spikes deal damage to the player on collision
 * - requires collider
 */
public class Spikes : DamagingHazards
{
	[Range(0,500)][SerializeField] float hurtForce = 5f;
	Collider2D coll;

	private new void Start() {
		coll = GetComponent<TilemapCollider2D>();
		base.Start();
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log("Colliidng");
		if(collision.gameObject.CompareTag("Player")) {
			Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
			//knock the player back in the opposite direction
		    //rb.AddForce(new Vector2(collision.transform.position.x - transform.position.x, 0f).normalized * hurtForce);
			DealDamage();
		}
	}
}

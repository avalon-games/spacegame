using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * DamagingHazards is the parent class for all environmental hazards that decrement the health of the player
 * - interacts with PlayerData and PlayerUI 
 * 
 */
public class DamagingHazards : MonoBehaviour
{
	PlayerController player;

	public void Start() {
		GameObject p = GameObject.FindGameObjectWithTag("Player");
		player = p.GetComponent<PlayerController>();
	}

	/**
	 * Causes player to take damage, reducing current hp and causing player to teleport to the last safe tile
	 */
	public void DealDamage() {
		player.TakeDamage();
	}
}

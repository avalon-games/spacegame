using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * DamagingHazards define a parent class for all environmental hazards that decrement the health of the player
 * 
 */
public class DamagingHazards : MonoBehaviour
{
	PlayerController player;
	PlayerUI ui;

	void Start() {
		GameObject p = GameObject.FindGameObjectWithTag("Player");
		ui = GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>();
		player = p.GetComponent<PlayerController>();
	}
	void DealDamage() {
		PlayerData.currHealth -= 1;
		ui.UpdateHealth();
		player.TeleportToLastSafeTile();
	}
}

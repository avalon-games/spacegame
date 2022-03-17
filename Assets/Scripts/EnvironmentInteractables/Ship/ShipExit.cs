using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipExit : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
			FindObjectOfType<SaveAndLoad>().LoadLevel(PlayerData.currUnlockedLevel);
		}
	}
}

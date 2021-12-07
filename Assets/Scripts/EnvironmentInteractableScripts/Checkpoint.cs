using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	SpriteRenderer sprite;
	bool isEnabled;

	private void Start() {
		sprite = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player") && !isEnabled) {
			PlayerData.checkpoint = transform.position;
			isEnabled = true;
			sprite.color = new Color(0.7f, 0.7f, 0.7f, 1);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	SpriteRenderer sprite;
	bool isEnabled;
	CheckpointManager cm;

	private void Start() {
		sprite = GetComponent<SpriteRenderer>();
		cm = transform.parent.gameObject.GetComponent<CheckpointManager>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player") && !isEnabled) {
			cm.ResetCheckpoints(); //reenable other checkpoints
			PlayerData.checkpoint = new float[2] { this.transform.position.x,this.transform.position.y };
			Debug.Log(this.transform.position.x + ", " + this.transform.position.y);
			isEnabled = true;
			sprite.color = new Color(0.5f, 0.5f, 0.5f, 1);
		}
	}
	public void ResetCheckpoint() {
		isEnabled = false;
		sprite.color = Color.white;
	}
}

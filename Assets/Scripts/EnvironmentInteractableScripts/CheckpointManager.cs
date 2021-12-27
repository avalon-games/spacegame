using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * allows player teleportation to the specified checkpoint in the scene
 */
public class CheckpointManager : MonoBehaviour
{
	List<Vector2> checkpoints;
	Transform player;

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		checkpoints = new List<Vector2>();
		foreach (Transform child in transform) {
			checkpoints.Add(child.position);
		}
	}
	/**
	 * Teleports the player to the specified checkpoint
	 */
	public void Teleport(int i) {
		player.position = checkpoints[i];
	}
	/**
	 * Returns true if the specified checkpoint exists
	 */
	public bool CheckpointExists(int i ) {
		return i >= 0 && i < checkpoints.Count;
	}

}

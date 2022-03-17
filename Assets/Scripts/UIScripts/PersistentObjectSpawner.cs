using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour {
	static bool hasSpawned = false;
	[SerializeField] GameObject persistentObjectPrefab;

	private void Awake() {
		if (hasSpawned) return;

		SpawnPersistentObjects();

		hasSpawned = true;
	}

	void SpawnPersistentObjects() {
		GameObject persistentObject = Instantiate(persistentObjectPrefab);
		DontDestroyOnLoad(persistentObject);
	}
}



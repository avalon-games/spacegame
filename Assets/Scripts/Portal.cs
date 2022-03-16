using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Portal : MonoBehaviour {
	[SerializeField] int sceneToLoad = -1;

	SceneChanger sc;

	int originScene;

	private void Awake() {
		originScene = SceneManager.GetActiveScene().buildIndex;
		
	}
	private void Start() {
		sc = FindObjectOfType<SceneChanger>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine(sc.Transition(sceneToLoad));
		}
	}
}

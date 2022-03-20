using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Portal : MonoBehaviour {
	[SerializeField] int sceneToLoad = -1;


	private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
			if (sceneToLoad < 0) {
				//-1 to automatically load the currently unlocked level
				FindObjectOfType<SaveAndLoad>().LoadLevel(PlayerData.currUnlockedLevel);
			} else {
				if (sceneToLoad > PlayerData.currUnlockedLevel) {
					PlayerData.currUnlockedLevel = sceneToLoad;
				}
				FindObjectOfType<SaveAndLoad>().LoadLevel(sceneToLoad);
			}
			
		}
	}
}

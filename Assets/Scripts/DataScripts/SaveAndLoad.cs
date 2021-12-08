using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Savestate saves the state of the player and loads the appropriate values of 
 * health,oxygen,level progress into PlayerData
 * - attaches to a SaveAndLoad game object
 * - handles saving data to file
 * - saving data between scenes is currently handled by PlayerData
 * - make sure the files level is indexed 0 in build settings
 */
public class SaveAndLoad : MonoBehaviour
{
	void Start() {
		//on entering a new scene, set the audio volume
		if (PlayerPrefs.HasKey("Volume"))
			PlayerData.volume = PlayerPrefs.GetFloat("Volume");
	}

	public void LoadGame(int i) {
		Load(i);
	}
	public void SaveGame(int i) {
		Save(i);
	}
	public void SavePreferences() {
		SavePref();
	}

	/**
	* Loads the current savefile values into PlayerData
	*/
	void Load(int i) {
		if (PlayerPrefs.HasKey("MaxHealth")) {
			Time.timeScale = 1;
			SaveFile.Load(i);


			SceneChanger.GoToLevel(PlayerData.currLevel);
			Debug.Log("Game data loaded!");
		}
	}


	/**
	* Saves the current game progress (in PlayerData) into a file
	* - in the future, add multiple save file functionality
	*/
	void Save(int i) {
		SaveFile.Save(i);
	}

	/*
	* Starts a new game, resetting all PlayerData values
	*/
	void NewGame() {
		Time.timeScale = 1;
		PlayerData.maxHealth = 5;
		PlayerData.maxOxygen = 100;
		PlayerData.currHealth = PlayerData.maxHealth;
		PlayerData.currOxygen = PlayerData.maxOxygen;
		PlayerData.currUnlockedLevel = 1;
		PlayerData.currLevel = 0;
		PlayerData.checkpoint = null;

		SceneChanger.GoToLevel(PlayerData.currLevel); //go to the first level
	}

	/**
		* Saves the volume, control, user options and preferences settings
		*/
	void SavePref() {
		PlayerPrefs.SetFloat("Volume", PlayerData.volume);
	}
}
//controls saving the player state to save files and transfer between scenes
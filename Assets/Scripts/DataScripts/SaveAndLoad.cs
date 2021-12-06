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


	/**
	* Loads the current savefile values into PlayerData
	*/
	public void LoadGame() {
		if (PlayerPrefs.HasKey("MaxHealth")) {
			Time.timeScale = 1;
			PlayerData.maxHealth = PlayerPrefs.GetInt("MaxHealth");
			PlayerData.maxOxygen = PlayerPrefs.GetInt("MaxOxygen");
			PlayerData.currHealth = PlayerPrefs.GetInt("CurrHealth");
			PlayerData.currOxygen = PlayerPrefs.GetInt("CurrOxygen");
			PlayerData.currUnlockedLevel = PlayerPrefs.GetInt("CurrUnlockedLevel");
			PlayerData.currLevel = PlayerPrefs.GetInt("CurrLevel");


			SceneChanger.GoToLevel(PlayerData.currLevel);
			Debug.Log("Game data loaded!");
		} else
			Debug.LogError("There is no save data!");
	}


	/**
	 * Saves the current game progress (in PlayerData) into a file
	 * - in the future, add multiple save file functionality
	 */
	public void SaveGame() {
		PlayerPrefs.SetInt("MaxHealth", PlayerData.maxHealth);
		PlayerPrefs.SetInt("MaxOxygen", PlayerData.maxOxygen);
		PlayerPrefs.SetInt("CurrHealth", PlayerData.currHealth);
		PlayerPrefs.SetInt("CurrOxygen", PlayerData.currOxygen);
		PlayerPrefs.SetInt("CurrUnlockedLevel", PlayerData.currUnlockedLevel);
		PlayerPrefs.SetInt("CurrLevel", PlayerData.currLevel);
	}

	/*
	 * Starts a new game, resetting all PlayerData values
	 */
	public void NewGame() {
		Time.timeScale = 1;
		PlayerData.maxHealth = 5;
		PlayerData.maxOxygen = 100;
		PlayerData.currHealth = PlayerData.maxHealth;
		PlayerData.currOxygen = PlayerData.maxOxygen;
		PlayerData.currUnlockedLevel = 1;
		PlayerData.currLevel = 0;

		SceneChanger.GoToLevel(PlayerData.currLevel); //go to the first level

		SaveGame();
	}

	/**
	 * Saves the volume, control, user options and preferences settings
	 */
	public void SavePreferences() {
		PlayerPrefs.SetFloat("Volume", PlayerData.volume);
	}
}
//controls saving the player state to save files and transfer between scenes
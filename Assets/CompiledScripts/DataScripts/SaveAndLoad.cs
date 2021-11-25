using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Savestate saves the state of the player and loads the appropriate values of 
 * health,oxygen,level progress into PlayerData
 * - handles saving data to file
 * - saving data between scenes is currently handled by PlayerData
 * - make sure the files level is indexed 0 in build settings
 */
public class SaveAndLoad : MonoBehaviour
{


	void Start() {
		
	}
	/**
	* Loads the current savefile values into PlayerData
	*/
	public void LoadGame() {
		if (PlayerPrefs.HasKey("MaxHealth")) {
			PlayerData.maxHealth = PlayerPrefs.GetInt("MaxHealth");
			PlayerData.maxOxygen = PlayerPrefs.GetFloat("MaxOxygen");
			PlayerData.currHealth = PlayerPrefs.GetInt("CurrHealth");
			PlayerData.currOxygen = PlayerPrefs.GetFloat("CurrOxygen");
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
		PlayerPrefs.SetFloat("MaxOxygen", PlayerData.maxOxygen);
		PlayerPrefs.SetInt("CurrHealth", PlayerData.currHealth);
		PlayerPrefs.SetFloat("CurrOxygen", PlayerData.currOxygen);
		PlayerPrefs.SetInt("CurrUnlockedLevel", PlayerData.currUnlockedLevel);
		PlayerPrefs.SetInt("CurrLevel", PlayerData.currLevel);
	}

	/*
	 * Starts a new game, resetting all PlayerData values
	 */
	public void NewGame() {
		PlayerData.maxHealth = 5;
		PlayerData.maxOxygen = 100f;
		PlayerData.currHealth = PlayerData.maxHealth;
		PlayerData.currOxygen = PlayerData.maxOxygen;
		PlayerData.currUnlockedLevel = 1;
		PlayerData.currLevel = 0;

		SceneChanger.GoToLevel(PlayerData.currLevel);

		SaveGame();
	}

	/**
	 * Saves the volume, control, user options and preferences settings
	 */
	public void SavePreferences() {

	}
}
//controls saving the player state to save files and transfer between scenes
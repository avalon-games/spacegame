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
		//set the text of savefiles
		if (PlayerPrefs.HasKey("File0")) {
			//this currently doesn't work in editor, as player prefs aren't preserved in editor after exiting play mode
			//but should work in build
			Debug.Log("Loading file names!!");
			PlayerData.saveFileNames = new string[4];
			PlayerData.saveFileNames[0] = PlayerPrefs.GetString("File0");
			PlayerData.saveFileNames[1] = PlayerPrefs.GetString("File1");
			PlayerData.saveFileNames[2] = PlayerPrefs.GetString("File2");
			PlayerData.saveFileNames[3] = PlayerPrefs.GetString("File3");
		}

	}

	public void LoadGame(int i) {
		Load(i);
	}
	public void SaveGame(int i) {
		Save(i);
		SaveFileNames();
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
			Debug.Log("Loading file " + i);
		}
	}


	/**
	* Saves the current game progress (in PlayerData) into a file
	* - in the future, add multiple save file functionality
	*/
	void Save(int i) {
		SaveFile.Save(i);
		SaveFileNames();
		Debug.Log("Saving file " + i);
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

	//the savefile names are also saved using PlayerPrefs as they should contain the same values accross all save files
	void SaveFileNames() {
		PlayerPrefs.SetString("File0", PlayerData.saveFileNames[0]);
		PlayerPrefs.SetString("File1", PlayerData.saveFileNames[1]);
		PlayerPrefs.SetString("File2", PlayerData.saveFileNames[2]);
		PlayerPrefs.SetString("File3", PlayerData.saveFileNames[3]);
	}


}
//controls saving the player state to save files and transfer between scenes
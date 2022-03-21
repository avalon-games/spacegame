using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	private void Awake() {
		InitData();
	}
	void Start() {
		
		//on entering a new scene, set the audio volume
		if (PlayerPrefs.HasKey("Volume"))
			PlayerData.volume = PlayerPrefs.GetFloat("Volume");
		//set the text of savefiles
		if (PlayerPrefs.HasKey("File0")) {
			//this currently doesn't work in editor, as player prefs aren't preserved in editor after exiting play mode
			//but should work in build
			PlayerData.saveFileNames = new string[4];
			PlayerData.saveFileNames[0] = PlayerPrefs.GetString("File0");
			PlayerData.saveFileNames[1] = PlayerPrefs.GetString("File1");
			PlayerData.saveFileNames[2] = PlayerPrefs.GetString("File2");
			PlayerData.saveFileNames[3] = PlayerPrefs.GetString("File3");
		}
		PlayerData.currLevel = FindObjectOfType<SceneChanger>().GetCurrScene();

	}

	public void LoadGame(int i) {
		print("loading game...");
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
		if (!PlayerData.saveFileNames[i].Equals("Empty")) {
			Time.timeScale = 1;
			SaveFile.Load(i);
			LoadLevel(PlayerData.currLevel);
		}
	}
	public void LoadLevel(int level) {
		print("loading" + level);
		StartCoroutine(LoadLevelAsync(level));
	}

	IEnumerator LoadLevelAsync(int level) {
		yield return FindObjectOfType<SceneChanger>().Transition(level);
	}


	/**
	* Saves the current game progress (in PlayerData) into a file
	* - in the future, add multiple save file functionality
	*/
	void Save(int i) {
		SaveFile.Save(i);
		SaveFileNames();
		//Debug.Log("Saving file " + i);
	}

	/*
	* Starts a new game, resetting all PlayerData values
	*/
	public void NewGame() {
		InitData();

		LoadLevel(PlayerData.currLevel);
	}

	/**
	* Saves the volume, control, user options and preferences settings
	*/
	void SavePref() {
		PlayerPrefs.SetFloat("Volume", PlayerData.volume);
	}

	//the savefile names are also saved using PlayerPrefs as they should contain the same values accross all save files
	void SaveFileNames() {
		string sceneName = SceneManager.GetActiveScene().name;
		if (sceneName == "ship" || sceneName == "title") {
			return;
		}
		PlayerPrefs.SetString("File0", PlayerData.saveFileNames[0]);
		PlayerPrefs.SetString("File1", PlayerData.saveFileNames[1]);
		PlayerPrefs.SetString("File2", PlayerData.saveFileNames[2]);
		PlayerPrefs.SetString("File3", PlayerData.saveFileNames[3]);
	}

	void InitData() {
		Time.timeScale = 1;
		PlayerData.maxHealth = 5;
		PlayerData.maxOxygen = 100;
		PlayerData.currHealth = PlayerData.maxHealth;
		PlayerData.currOxygen = PlayerData.maxOxygen;
		PlayerData.currUnlockedLevel = 2;
		PlayerData.currLevel = 1; //0 is title scene, 1 is spaceship
		PlayerData.checkpoint = null;
	}

}
//controls saving the player state to save files and transfer between scenes
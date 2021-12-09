using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

/**
 * Class for the save file UI buttons to determine whether to save or load the file
 * is responsible for saving and displaying the savefile text
 * make sure to update the PlayerData.inGameTime on each level load to have accurate times
 */
public class SaveOrLoadButton : MonoBehaviour
{
    public SaveAndLoad sl;
    public UIMenus uiMenus;
	public TextMeshProUGUI text;
	[Range(0,3)] public int buttonNumber; //each button should have a distinct number

	private void OnEnable() {
		Debug.Log("button" + buttonNumber + " is enabled");
		if (PlayerData.saveFileNames == null)
			PlayerData.saveFileNames = new string[] { "Empty", "Empty", "Empty", "Empty" };
		text.SetText(PlayerData.saveFileNames[buttonNumber]);
	}

	public void SaveOrLoad() {
        if (uiMenus.isSaving) {
			PlayerData.inGameTime += Time.timeSinceLevelLoad;
			PlayerData.saveFileNames[buttonNumber] = SceneManager.GetActiveScene().name + " " + TimeFormatter(PlayerData.inGameTime);
			
			text.SetText(PlayerData.saveFileNames[buttonNumber]);

			sl.SaveGame(buttonNumber);
			Debug.Log("setting text");
		} else {
            sl.LoadGame(buttonNumber);
		}
	}

	string TimeFormatter(float seconds, bool forceHHMMSS = false) {
		int secondsRemainder = (int) seconds % 60;
		int minutes = ((int)(seconds / 60)) % 60;
		int hours = (int)(seconds / 3600);

		if (!forceHHMMSS) {
			if (hours == 0) {
				return minutes.ToString("D2") + ":" + secondsRemainder.ToString("D2"); ;
			}
		}
		return hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + secondsRemainder.ToString("D2"); ;
	}
}

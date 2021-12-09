using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/*
 * Controls all UI menus: pause,options
 * press esc to enter pause menu
 */
public class UIMenus : MonoBehaviour {
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject saveLoadMenu;
    public SaveAndLoad save;

    public AudioMixer mixer; //make sure to drag it in

    public PlayerUI playerUI;
    [HideInInspector] public bool isSaving; //is loading if false, used to control button behavior in SaveOrLoadButton


    void Start() {
        //for testing only
        PlayerData.maxOxygen = 100;
        PlayerData.currOxygen = PlayerData.maxOxygen;
        //////////////
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        saveLoadMenu.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pauseMenu.activeSelf) {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            } else if (optionsMenu.activeSelf) {
                save.SavePreferences();
                pauseMenu.SetActive(true);
                optionsMenu.SetActive(false);
            } else if (saveLoadMenu.activeSelf) {
                pauseMenu.SetActive(true);
                saveLoadMenu.SetActive(false);
            } else {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
                
        }
    }

    #region PauseMenu

    /**
     * toggles pausemenu on and off
     */
    public void TogglePauseMenu() {
        if (pauseMenu.activeSelf) {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        } else {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
    }

    /**
     * Quits the game
     */
    public void QuitGame() {
        Application.Quit();
    }

    /**
     * Loads title scene
     */
    public void LoadMainMenu() {
        Time.timeScale = 1;
        SceneChanger.GoToMainMenu();
    }

    #endregion

    /**
     * Toggles the options menu active or inactive based on current state
     */
    public void ToggleOptionsMenu() {
        if (optionsMenu.activeSelf) {
            save.SavePreferences();
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
        } else {
            optionsMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }
    }

    /**
     * Is called when volume changes
     */
    void SetVolume(float volume) {
        PlayerData.volume = volume;
        mixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
    }

    /**
     * For testing only, so player doesn't die
     */
    public void FreeHeal() {
        PlayerData.maxHealth = 5;
        PlayerData.maxOxygen = 100;
        PlayerData.currHealth = PlayerData.maxHealth;
        PlayerData.currOxygen = PlayerData.maxOxygen;
        playerUI.UpdateHealth();
        playerUI.UpdateOxygen();
    }

    public void ToggleSaveMenu() {
        if (saveLoadMenu.activeSelf) {
            pauseMenu.SetActive(true);
            saveLoadMenu.SetActive(false);
        } else {
            saveLoadMenu.SetActive(true);
            pauseMenu.SetActive(false);
            isSaving = true;
        }
    }
    public void ToggleLoadMenu() {
        if (saveLoadMenu.activeSelf) {
            pauseMenu.SetActive(true);
            saveLoadMenu.SetActive(false);
        } else {
            saveLoadMenu.SetActive(true);
            pauseMenu.SetActive(false);
            isSaving = false;
        }
    }

}

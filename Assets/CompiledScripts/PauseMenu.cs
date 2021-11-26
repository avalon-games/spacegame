using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Controls everything that happens inside the pause menu
 * press esc to enter pause menu
 */
public class PauseMenu : MonoBehaviour {
    bool isPaused;
    GameObject pauseMenu;

    void Start() {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        Resume();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePauseMenu();
        }
    }

    /**
     * Exits Pause Menu
     */
    void Resume() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
	}

    /**
     * Enter pause menu
     */
    void Pause() {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isPaused = true;
	}

    /**
     * Determine whether to enter or exit pause menu
     */
    public void TogglePauseMenu() {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    /**
     * Manages interactions in the options menu
     */
    public void OptionsMenu() {

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
}

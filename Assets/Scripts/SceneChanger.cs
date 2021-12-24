using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

/**
 * Manages transition between scenes
 */
public static class SceneChanger
{
    public static void GoToMainMenu() {
        SceneManager.LoadScene("TitleScene");
    }

    public static void GoToSpaceship() {
        SceneManager.LoadScene("Spaceship");
    }

    public static void GoToLevel(int target) {
        if (target < SceneManager.sceneCountInBuildSettings && target >= 0)
            SceneManager.LoadScene(target);
        else
            Debug.LogError("Scene " + target + " is not in the build settings");
    }

    public static int GetCurrScene() {
        return SceneManager.GetActiveScene().buildIndex;
	}

    public static List<string> GetBuildScenes() {
        List<string> scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
            if (scene.enabled)
                scenes.Add(scene.path);
        }
        return scenes;
    }

    public static int GetCurrScene() {
        return SceneManager.GetActiveScene().buildIndex;
	}
}
//add scene change commands
//lose hp should be taken care off in hazard script
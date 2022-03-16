using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;


/**
 * Manages transition between scenes
 */
public class SceneChanger : MonoBehaviour {
    [SerializeField] float fadeOutTime = 1f;
    [SerializeField] float fadeInTime = 2f;
    [SerializeField] float fadeWaitTime = 0.5f;

    public void GoToMainMenu() {
        SceneManager.LoadScene("TitleScene");
    }

    public void GoToSpaceship() {
        SceneManager.LoadScene("Spaceship");
    }

    public void GoToLevel(int target) {
        if (target < SceneManager.sceneCountInBuildSettings && target >= 0) {
            PlayerData.checkpoint = null;
            SceneManager.LoadSceneAsync(target);
        } else
            Debug.LogError("Scene " + target + " is not in the build settings");
    }

    public int GetCurrScene() {
        return SceneManager.GetActiveScene().buildIndex;
	}

    // Change scene with transition effects
    public IEnumerator Transition(int sceneToLoad) {
        print("Loading Level...");
        if (sceneToLoad < 0) {
            Debug.LogError("SceneToLoad not set");
            yield break;
        }

        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutTime);
        SaveAndLoad sal = FindObjectOfType<SaveAndLoad>();
        sal.SaveGame(0);

        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        UpdatePlayer();

        yield return new WaitForSeconds(fadeWaitTime);
        yield return fader.FadeIn(fadeInTime);
    }

    void UpdatePlayer() {
        if (PlayerData.checkpoint == null)
            SetCheckpointToStart();
        else {
            SpawnAtCheckpoint();
        }
    }

    private void SpawnAtCheckpoint() {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        player.transform.position = new Vector2(PlayerData.checkpoint[0], PlayerData.checkpoint[1]);
    }

    private void SetCheckpointToStart() {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerData.checkpoint = new float[2] { player.transform.position.x, player.transform.position.y };
    }



#if UNITY_EDITOR
    public List<string> GetBuildScenes() {
        List<string> scenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
            if (scene.enabled)
                scenes.Add(scene.path);
        }
        return scenes;
    }
    #endif
}

//add scene change commands
//lose hp should be taken care off in hazard script
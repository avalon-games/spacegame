using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Manages transition between scenes
 */
public static class SceneChanger
{

    public static void GoToSpaceship() {
        SceneManager.LoadScene("Spaceship");
    }

    public static void GoToLevel(int target) {
        SceneManager.LoadScene(target);
    }
}
//add scene change commands
//lose hp should be taken care off in hazard script
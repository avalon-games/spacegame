using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    void GoToSpaceship() {
        SceneManager.LoadScene("Spaceship");
    }

    void GoToLevel(string target) {
        SceneManager.LoadScene(target);
    }
}
//add scene change commands
//lose hp should be taken care off in hazard script
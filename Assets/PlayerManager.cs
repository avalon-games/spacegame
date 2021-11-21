using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private HeartBar playerHealth;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseHeart()
    {
        if (!Spacegame.HealthLockFree)
        {
            return;
        }

        Spacegame.HealthLockFree = false;

        playerHealth.removeHeart();
        
        if (playerHealth.getHealth() <= 0)
        {
            SpawnToSpaceship();
        }
        else
        {
            SpawnToCheckpoint();
        }

        Spacegame.HealthLockFree = true;
    }

    private void SpawnToSpaceship()
    {
        SceneManager.LoadScene("Level 1 Scene");
    }

    private void SpawnToCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

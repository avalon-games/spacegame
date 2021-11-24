using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private HeartBar playerHealth;

    public void LoseHeart()
    {
        playerHealth.removeHeart();

        if (playerHealth.getHealth() <= 0)
        {
            SpawnToSpaceship();
        }
        else
        {
            SpawnToCheckpoint();
        }
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

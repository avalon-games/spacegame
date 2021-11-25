using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private static bool healthLockFree; //allows current player health to decriment
    private static int count = 0;
    private int id;

    private void Start()
    {
        healthLockFree = true;
        id = count++;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && healthLockFree)
        {
            healthLockFree = false;
            Debug.Log("reducing health - hazard script");
            collision.gameObject.GetComponent<PlayerManager>().LoseHeart();
        }
    }
}

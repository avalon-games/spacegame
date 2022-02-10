using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Class for quicksand - disables player movement control while inside
 */
public class QuickSand : MonoBehaviour
{
    PlayerController player;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            player.isInQuicksand = true;
        }
    }
    /**
     * OnTriggerExit2D is called when a collision with a trigger is detected
     * Current Usage:
     * - changing player velocity when exiting wind
     */
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
            player.isInQuicksand = false;
            
    }
}

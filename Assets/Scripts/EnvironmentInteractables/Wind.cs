using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Wind pushes the player back in one direction and modifies the player's max movement speed based on wind direction
 * - requires area effector
 */
public class Wind : MonoBehaviour
{
    //[Range(0, 10f)] [SerializeField] float maxVelocityChange;
    //float angle;
    //PlayerController player;

    //void Start() {
    //   angle = GetComponent<AreaEffector2D>().forceAngle;
    //   player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    //}
    ///**
    //  * OnTriggerEnter2D is called when a collision with a trigger is detected
    //  * Current Usage:
    //  * - changing player velocity when entering wind
    //  */
    //void OnTriggerEnter2D(Collider2D collision) {
    //    if (collision.gameObject.CompareTag("Player")) {
    //        player.maxSpeedLeft += Mathf.Cos(angle * Mathf.PI / 180) * maxVelocityChange;
    //        player.maxSpeedRight += Mathf.Cos(angle * Mathf.PI / 180) * maxVelocityChange;
    //    }
    //}
    ///**
    // * OnTriggerExit2D is called when a collision with a trigger is detected
    // * Current Usage:
    // * - changing player velocity when exiting wind
    // */
    //void OnTriggerExit2D(Collider2D collision) {
    //    if (collision.gameObject.CompareTag("Player")) {
    //        player.maxSpeedLeft = -player.initialMaxSpeed;
    //        player.maxSpeedRight = player.initialMaxSpeed;
    //    }
    //}
}

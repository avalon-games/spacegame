using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * A class of game objects that breaks when the player steps on it
 * requires boxcollider and sprite
 */
public class Breakable : MonoBehaviour
{
    Collider2D coll;
    TilemapRenderer tilemap;

    void Start() {

        coll = GetComponent<TilemapCollider2D>();
        tilemap = GetComponent<TilemapRenderer>();
    }

    public void DestroyBlock() {
        StartCoroutine(DestroyAndWait());
    }

    /**
     * DestroyAndWait - disables the collider and sprite after a few seconds, 
     * then respawn the block after a few seconds
     */
    IEnumerator DestroyAndWait() {
        yield return new WaitForSeconds(2f); //time to destroy
        coll.enabled = false;
        tilemap.enabled = false; //make transparent
        yield return new WaitForSeconds(5f); //time to respawn
        coll.enabled = true;
        tilemap.enabled = true;
    }
    /**
     * OnCollisionEnter2D is called when colliding with a collider
     * Current Usage:
     * - Detecting if player collides with a destructible block and breaks itself if true
     */
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Destroying block");
        if (collision.gameObject.CompareTag("Player")) {
            this.DestroyBlock();
        }
    }
}

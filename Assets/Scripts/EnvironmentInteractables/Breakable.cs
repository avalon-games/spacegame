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
    SpriteRenderer sprite;

    void Start() {

        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void DestroyBlock() {
        StartCoroutine(DestroyAndWait());
    }

    /**
     * DestroyAndWait - disables the collider and sprite after a few seconds, 
     * then respawn the block after a few seconds
     */
    IEnumerator DestroyAndWait() {
        yield return new WaitForSeconds(2f); //time until destroy
        coll.enabled = false;
        sprite.color = new Color(1,1,1,0.2f); //make transparent
        yield return new WaitForSeconds(5f); //time until respawn
        coll.enabled = true;
        sprite.color = new Color(1, 1, 1, 1); 
    }
    /**
     * OnCollisionEnter2D is called when colliding with a collider
     * Current Usage:
     * - Detecting if player collides with a destructible block and breaks itself if true
     */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            this.DestroyBlock();
        }
    }
}

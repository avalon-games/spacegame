using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        yield return new WaitForSeconds(2f); //time to destroy
        coll.enabled = false;
        sprite.enabled = false; //make transparent
        yield return new WaitForSeconds(5f); //time to respawn
        coll.enabled = true;
        sprite.enabled = true;
    }
    /**
     * OnCollisionEnter2D is called when colliding with a collider
     * Current Usage:
     * - Detecting if player collides with a destructible block and breaks itself if true
     */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y > this.transform.position.y) {
            this.DestroyBlock();
        }
    }
}

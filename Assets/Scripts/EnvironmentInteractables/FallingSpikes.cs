using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Spikes that fall when players enter its downwards raycast
 * - requires rigidbody
 */
public class FallingSpikes : Spikes
{
    [Range (0,20)][SerializeField] float detectionRange = 20f;
    float initialYPosition;
    Rigidbody2D rb;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        initialYPosition = transform.position.y;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down * detectionRange);

        if (hit.collider != null && hit.collider.gameObject.tag == "Player") {
            rb.gravityScale = 1;
        }
        if (transform.position.y < (initialYPosition - 2*detectionRange))
            Destroy(this.gameObject);
    }
    /**
     * Drawing Gizmos in edit mode to display the raycast for detecting ground
     */
    [ExecuteInEditMode]
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - detectionRange));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDown : MonoBehaviour
{
    [Range(0, 5)][SerializeField] float speed;
    Rigidbody2D rb;
    float horizontalDir;
    float verticalDir;
    bool isBusy;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
	private void Update() {
        verticalDir = Input.GetAxisRaw("Vertical");
        horizontalDir = Input.GetAxisRaw("Horizontal");
    }

	void FixedUpdate() {
        Vector2 targetVelocity = Vector2.zero;
        if (!isBusy) {
            if (verticalDir == 1)
                targetVelocity = new Vector2(targetVelocity.x, speed);
            else if (verticalDir == 0)
                targetVelocity = new Vector2(targetVelocity.x, 0);
            else
                targetVelocity = new Vector2(targetVelocity.x, -speed);
            if (horizontalDir == 1)
                targetVelocity = new Vector2(speed, targetVelocity.y);
            else if (horizontalDir == 0)
                targetVelocity = new Vector2(0, targetVelocity.y);
            else
                targetVelocity = new Vector2(-speed, targetVelocity.y);

            rb.velocity = targetVelocity;
        }
    }
    
}

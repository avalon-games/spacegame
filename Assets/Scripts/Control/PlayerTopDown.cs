using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDown : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody2D rb;
    Animator animator;
    float horizontalDir;
    float verticalDir;
    bool isBusy;

    enum State { idle, running, jumping, falling, pushing, hurt }; //animation states, decides interactions
    State state;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0;
    }
	private void Update() {
        verticalDir = Input.GetAxisRaw("Vertical");
        horizontalDir = Input.GetAxisRaw("Horizontal");
        SetAnimation();
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

    void SetAnimation() {
        if (rb.velocity != Vector2.zero) {
            state = State.running;
        } else
            state = State.idle;

        animator.SetInteger("state", (int)state);

        if (horizontalDir < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
		} else if (horizontalDir > 0 ) {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    
}

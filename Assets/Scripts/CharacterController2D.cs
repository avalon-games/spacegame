using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    
    Rigidbody2D rb;
    Collider2D coll;
    LayerMask groundLayer;
    //LayerMask enemyLayer;
    //Animator animator;
    enum State {idle, running, jumping, falling, hurt}; //animation states, decides interactions
    State state;
    [SerializeField] int playerSpeed = 5;
    [SerializeField] float jumpForce = 300f;
    [SerializeField] float airControl = 0.8f;
    //[SerializeField] float hurtForce = 2f;
    [SerializeField] float groundDetectRadius = 0.24f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .2f;
    Vector3 m_Velocity = Vector3.zero;

    float horizontalInput;
    bool jumpButton;
    bool isOnGround;

    void Start() { 
        state = State.idle;
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");
        //enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) jumpButton = true;
        RaycastHit2D groundHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y + groundDetectRadius, groundLayer);
        Debug.DrawRay(coll.bounds.center, Vector2.down * (coll.bounds.extents.y + groundDetectRadius)); 
        isOnGround = groundHit.collider != null;

        AssignState();
        //animator.SetInteger("state", (int)state);

        
    }

    void FixedUpdate() {
        MovePlayer();
    }

    void MovePlayer() {
        Vector3 targetVelocity;
        if(state != State.hurt) {
            if (isOnGround) {
                if (horizontalInput < 0)
                    targetVelocity = new Vector2(-playerSpeed, rb.velocity.y);
                else if (horizontalInput > 0)
                    targetVelocity = new Vector2(playerSpeed, rb.velocity.y);
                else
                    targetVelocity = new Vector2(0, rb.velocity.y);
                if (jumpButton) {
                    rb.AddForce(new Vector2(0f, jumpForce));
                    isOnGround = false;
                    jumpButton = false;
                }
            }
            else {
                if (horizontalInput < 0)
                    targetVelocity = new Vector2(-playerSpeed*airControl, rb.velocity.y);
                else if (horizontalInput > 0)
                    targetVelocity = new Vector2(playerSpeed*airControl, rb.velocity.y);
                else
                    targetVelocity = new Vector2(0, rb.velocity.y);
            }
            if (horizontalInput < 0)
                transform.localScale = new Vector2(-1, 1);
            else if (horizontalInput > 0)
                transform.localScale = new Vector2(1, 1);

			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
    }
    void AssignState() {
        //if (hurt)
        //    state = State.hurt;
        if (rb.velocity.y > 0)
            state = State.jumping;
        else if (rb.velocity.y < 0)
            state = State.falling;
        else if (Mathf.Abs(horizontalInput) > 0)
            state = State.running;
        else
            state = State.idle;
    }
}

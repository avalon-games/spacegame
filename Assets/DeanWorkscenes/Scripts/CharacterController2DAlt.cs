using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2DAlt : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D coll;
    SpriteRenderer sprite;
    LayerMask groundLayer;
    //LayerMask enemyLayer;
    //Animator animator;
    enum State {idle, running, jumping, falling, hurt}; //animation states, decides interactions
    State state;

    [Header("Animation")]
    [SerializeField] Animator animator;

    [Header("Horizontal:")]
    float horizontalInput;
    [Range(0, 20f)] [SerializeField] float initialMaxSpeed = 10;
    float maxSpeedLeft;
    float maxSpeedRight;
    [Range(0, 10f)][SerializeField] float maxSpeedDelta = 4; //later on may want to make this dependent on force magnitude of wind
    [Range(0, .3f)][SerializeField] float movementSmoothTime = .2f;
    Vector3 m_Velocity = Vector3.zero;

    [Header("Jump & Midair:")]
    bool jumpButton;
    bool isOnGround;
    [Range(0, 3000f)][SerializeField] float jumpForce = 1000f;
    [Range(-5, 0f)] [SerializeField] float earlyJumpReleaseYVelocity = -1f;
    [Range(0, 1f)][SerializeField] float airControl = 0.8f;
    //[SerializeField] float hurtForce = 2f;
    [Range(0, .5f)][SerializeField] float groundDetectRadius = 0.24f;
    [HideInInspector] public bool jumpReleaseActive = true;

    bool isInQuicksand;
    LayerMask sandLayer;

    private bool movementAllowed = true;

    void Start() {
        maxSpeedLeft = -initialMaxSpeed;
        maxSpeedRight = initialMaxSpeed;

        sprite = GetComponent<SpriteRenderer>();
        state = State.idle;
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");
        sandLayer = LayerMask.GetMask("Sand");
        //enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update() {
        if (!movementAllowed)
        {
            return;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (jumpButton == false && Input.GetButtonDown("Jump")) jumpButton = true;
        if (jumpButton == true && Input.GetButtonUp("Jump")) jumpButton = false;
        DetectGround();
        DetectQuicksand();

        //animations based on current state of player
		    AssignState();
		    //animator.SetInteger("state", (int)state);
	}


    void FixedUpdate() {
        if (!movementAllowed)
        {
            return;
        }
        MovePlayer();
    }

    void MovePlayer() {
        Vector2 targetVelocity = Vector2.zero;
        if (state != State.hurt) {
            if (horizontalInput < 0)
                targetVelocity = new Vector2(maxSpeedLeft, rb.velocity.y);
            else if (horizontalInput > 0)
                targetVelocity = new Vector2(maxSpeedRight, rb.velocity.y);
            else
                targetVelocity = new Vector2(0, rb.velocity.y);
            if (isInQuicksand) {
                rb.velocity = Vector2.zero;
                if (jumpButton)
                    rb.AddForce(new Vector2(0f, jumpForce * 0.1f));
            }
            else if (isOnGround && jumpButton)
            {
                animator.Play("Jump");
                rb.AddForce(new Vector2(0f, jumpForce));
                isOnGround = false;
            }

            if (horizontalInput < 0)
                sprite.flipX = true;
            else if (horizontalInput > 0)
                sprite.flipX = false;
        }
        //variable jump height
        if (!jumpButton && rb.velocity.y > 0.5f && jumpReleaseActive) 
            targetVelocity.y = earlyJumpReleaseYVelocity;
        //smooths out player velocity change
		if (isOnGround)
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothTime);
        else
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothTime/airControl);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        //change velocity when entering wind
        if (collision.gameObject.CompareTag("Wind")) {
            float angle = collision.gameObject.GetComponent<AreaEffector2D>().forceAngle;
            maxSpeedLeft += Mathf.Cos(angle * Mathf.PI / 180) * maxSpeedDelta;
            maxSpeedRight += Mathf.Cos(angle * Mathf.PI/ 180) * maxSpeedDelta;
        }
        //if (collision.gameObject.CompareTag("Hurtful")) {
        //    //decrease health
        //}
    }
    void OnTriggerExit2D(Collider2D collision) {
        //change velocity on exiting wind
        if (collision.gameObject.CompareTag("Wind")) {
            maxSpeedLeft = -initialMaxSpeed;
            maxSpeedRight = initialMaxSpeed;
        }
    }

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Destructible Block")) {
            DestructibleBlock db = collision.gameObject.GetComponent<DestructibleBlock>();
			db.DestroyBlock();
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

    private void DetectGround() {
        RaycastHit2D groundHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y + groundDetectRadius, groundLayer);
        Debug.DrawRay(coll.bounds.center, Vector2.down * (coll.bounds.extents.y + groundDetectRadius));
        isOnGround = groundHit.collider != null;
    }
    private void DetectQuicksand() {
        RaycastHit2D sandHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y + groundDetectRadius, sandLayer);
        isInQuicksand = sandHit.collider != null;
    }
    public void EnableMovement(bool _movementAllowed)
    {
        movementAllowed = _movementAllowed;
    }
}
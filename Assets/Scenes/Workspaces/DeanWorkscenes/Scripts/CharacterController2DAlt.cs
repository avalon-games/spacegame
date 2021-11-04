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
    float defaultGravity;


    [Header("Grappling:")]
    public Tutorial_GrapplingGun grappleGun;
    //[Range(0, 500)] [SerializeField] float wallHopForceX = 200;
    //[Range(0, 500)] [SerializeField] float wallHopForceY = 200;
    //bool isOnRightWall;
    //bool isOnLeftWall;

    void Start() {
        maxSpeedLeft = -initialMaxSpeed;
        maxSpeedRight = initialMaxSpeed;

        sprite = GetComponent<SpriteRenderer>();
        state = State.idle;
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");
        //enemyLayer = LayerMask.GetMask("Enemy");

        defaultGravity = rb.gravityScale;
    }

    void Update() {
		horizontalInput = Input.GetAxisRaw("Horizontal");
		if (jumpButton == false && Input.GetButtonDown("Jump")) jumpButton = true;
		if (jumpButton == true && Input.GetButtonUp("Jump")) jumpButton = false;
		DetectGround();
        //if(grappleRope.isGrappling) //this is for wallhop
        //    DetectWall();

		AssignState();
		//animator.SetInteger("state", (int)state);
	}

	void FixedUpdate() {
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
            if (isOnGround && jumpButton) {
                rb.AddForce(new Vector2(0f, jumpForce));
                isOnGround = false;
            }
            if (horizontalInput < 0)
                sprite.flipX = true;
            else if (horizontalInput > 0)
                sprite.flipX = false;
        }
        if (!jumpButton && rb.velocity.y > 0.5f && !grappleGun.grappleRope.isGrappling) //if releasing the jump key early, have a shorter jump
            targetVelocity.y = earlyJumpReleaseYVelocity;

		if (grappleGun.grappleRope.isGrappling)
			rb.gravityScale = 0;
		else
			rb.gravityScale = defaultGravity;

		//if (isOnLeftWall) //wallhop
		//    rb.AddForce(new Vector2(wallHopForceX, wallHopForceY));
		//else if (isOnRightWall)
		//    rb.AddForce(new Vector2(-wallHopForceX, wallHopForceY));

		if (isOnGround)
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothTime);
        else
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothTime/airControl);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Wind")) {
            float angle = collision.gameObject.GetComponent<AreaEffector2D>().forceAngle;
            maxSpeedLeft += Mathf.Cos(angle * Mathf.PI / 180) * maxSpeedDelta;
            maxSpeedRight += Mathf.Cos(angle * Mathf.PI/ 180) * maxSpeedDelta;
        }
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Wind")) {
            maxSpeedLeft = -initialMaxSpeed;
            maxSpeedRight = initialMaxSpeed;
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
    //private void DetectWall() {
    //    RaycastHit2D rightWallHit = Physics2D.Raycast(coll.bounds.center, Vector2.right, coll.bounds.extents.x + groundDetectRadius, groundLayer);
    //    Debug.DrawRay(coll.bounds.center, Vector2.right * (coll.bounds.extents.x + groundDetectRadius));
    //    isOnRightWall = rightWallHit.collider != null;

    //    RaycastHit2D leftWallHit = Physics2D.Raycast(coll.bounds.center, Vector2.left, coll.bounds.extents.x + groundDetectRadius, groundLayer);
    //    Debug.DrawRay(coll.bounds.center, Vector2.left * (coll.bounds.extents.x + groundDetectRadius));
    //    isOnLeftWall = leftWallHit.collider != null;
    //}
}

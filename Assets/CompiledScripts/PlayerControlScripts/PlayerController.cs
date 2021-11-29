using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * PlayerController - controls player movement and animation states
 * Includes:
 * - player input monitoring
 * - horizontal and vertical movement speed control
 * - ground collision detection
 * - player teleportation
 * - player animation state assignment - an animator can set the animation based on current state
 */
public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D coll;
    SpriteRenderer sprite;
    enum State { idle, running, jumping, falling, hurt }; //animation states, decides interactions
    State state;

    [Header("Animation")]
    Animator animator;

    [Header("Horizontal:")]
    float horizontalInput;
    [Range(0, 20f)] public float initialMaxSpeed = 10;
    [HideInInspector] public float maxSpeedLeft;
    [HideInInspector] public float maxSpeedRight;
    [Range(0, .3f)] [SerializeField] float movementSmoothTime = .2f;
    Vector3 m_Velocity = Vector3.zero; //required for SmoothDamp
    private bool movementAllowed = true;

    [Header("Vertical:")]
    bool jumpButton;
    bool isOnGround;
    bool isInQuicksand;
    [Range(0, 3000f)] [SerializeField] float jumpForce = 1000f;
    [Range(-5, 0f)] [SerializeField] float earlyJumpReleaseYVelocity = -1f;
    [Range(0, 1f)] [SerializeField] float airControl = 0.8f;
    //[SerializeField] float hurtForce = 2f;
    [Range(0, 5f)] [SerializeField] float groundDetectRadius = 0.24f;
    [HideInInspector] public bool jumpReleaseActive = true;
    LayerMask groundLayer;
    LayerMask sandLayer;

    //in-scene teleportation
    Vector2 lastSafeTile;

    void Start() {
        maxSpeedLeft = -initialMaxSpeed;
        maxSpeedRight = initialMaxSpeed;

        sprite = GetComponent<SpriteRenderer>();
        state = State.idle;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");
        sandLayer = LayerMask.GetMask("Sand");
    }

    void Update() {
        if (!movementAllowed) {
            return;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (jumpButton == false && Input.GetButtonDown("Jump")) jumpButton = true;
        if (jumpButton == true && Input.GetButtonUp("Jump")) jumpButton = false;
        DetectBottomSurface();

        AssignState();
        //animator.SetInteger("state", (int)state);

        /////////////Debugging
        //if (Input.GetKeyDown(KeyCode.R))
        //    TeleportToLastSafeTile();
    }

    #region MainPlayerMovementControl
    void FixedUpdate() {
        if (!movementAllowed) {
            return;
        }
        MovePlayer();
    }

    /**
     * Player's movement controller
     * Includes: vertical/horizontal movement
     */
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
			} else if (isOnGround && jumpButton) {
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
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothTime / airControl);
	}
	public void EnableMovement(bool _movementAllowed) {
		movementAllowed = _movementAllowed;
	}
	#endregion

	#region EnvironmentalDetectionFunctions

    /**
     * Detects what surface the player is currently standing on
     */
	private void DetectBottomSurface() {
        //detects standing on ground
		RaycastHit2D groundHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, groundDetectRadius, groundLayer);

		isOnGround = groundHit.collider != null;
        lastSafeTile = new Vector2(groundHit.point.x, groundHit.point.y);

        //detects standing on quicksand
        RaycastHit2D sandHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y + groundDetectRadius, sandLayer);
        isInQuicksand = sandHit.collider != null;
    }

    /**
     * Drawing Gizmos in edit mode to display the raycast for detecting ground
     */
    [ExecuteInEditMode]
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y - groundDetectRadius));
    }
    #endregion

    /**
     * AssignState - assigns player animation based on current player movement state
     *     works in conjunction with the state variable and enum State
     */
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

    /**
     * Teleports the player to the last ground position that the player stood on
     * Used in: teleports player back to previous position on taking damage
     */
    public void TeleportToLastSafeTile() {
        transform.position = lastSafeTile;
        rb.velocity = Vector2.zero;
	}
}
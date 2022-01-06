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
    [HideInInspector] public Rigidbody2D rb;
    Collider2D coll;
    public SpriteRenderer sprite;
    PlayerUI ui;
    enum State { idle, running, jumping, falling, pushing, hurt }; //animation states, decides interactions
    State state;


    [Header("Animation")]
    Animator animator;

    [Header("Horizontal:")]
    float horizontalInput;
    [Range(0, 20f)] public float initialMaxSpeed = 5;
    [HideInInspector] public float maxSpeedLeft;
    [HideInInspector] public float maxSpeedRight;
    [Range(0, .3f)] [SerializeField] float movementSmoothTime = .2f;
    Vector3 m_Velocity = Vector3.zero; //required for SmoothDamp
    private bool movementAllowed = true;
    float initialScale;

    [Header("Vertical:")]
    bool jumpButton;
    [HideInInspector] public bool isOnGround;
    [HideInInspector] public bool isInQuicksand;
    [Range(0, 30f)] [SerializeField] float jumpHeight = 2f;

    [Range(-5, 0f)] [SerializeField] float earlyJumpReleaseYVelocity = -1f;
    [Range(-15f, 0)] [SerializeField] float maxDownVelocity = -10f;
    [Range(0, 1f)] [SerializeField] float airControl = 0.8f;
    //[SerializeField] float hurtForce = 2f;
    [Range(0, 5f)] [SerializeField] float groundDetectRadius = 1.08f;
    [HideInInspector] public bool jumpReleaseActive = true;
    LayerMask groundLayer;
    LayerMask sandLayer;

    //scene interactions
    bool invulnerable;
    [Range(0f, 5f)] [SerializeField] float invincibilityDuration = 2f;

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
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>();
        initialScale = transform.localScale.x;

        if (PlayerData.checkpoint == null)
            PlayerData.checkpoint = new float[2] { transform.position.x, transform.position.y };  //initial checkpoint is set to initial position
        else {
            //Debug.Log("Checkpoint load: " + PlayerData.checkpoint[0] + ", " + PlayerData.checkpoint[1]);
            transform.position = new Vector2(PlayerData.checkpoint[0], PlayerData.checkpoint[1]);
		}

    }

    void Update() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (jumpButton == false && Input.GetButtonDown("Jump")) jumpButton = true;
        if (jumpButton == true && Input.GetButtonUp("Jump")) jumpButton = false;
        DetectBottomSurface();

        //AssignState();
        animator.SetInteger("state", (int)state);

        sprite.color = (invulnerable) ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);


        /////////////Debugging
        //if (Input.GetKeyDown(KeyCode.R))
        //    TeleportToLastSafeTile();
    }

    #region MainPlayerMovementControl
    void FixedUpdate() {
        //limit downward velocity
        if (rb.velocity.y < maxDownVelocity) rb.velocity = new Vector2(rb.velocity.x, maxDownVelocity);
        //movement control
        if (movementAllowed) {
            MovePlayer();
        } 
    }

    /**
     * Player's movement controller
     * Includes: vertical/horizontal movement
     */
    void MovePlayer() {
        Vector2 targetVelocity = Vector2.zero;
        if (!movementAllowed) {
            return;
        }

        //v_0=2hv_x/x_h ----------   v_0= (h - 0.5gt_h^2) / t_h
        float timeToMaxHeight = Mathf.Sqrt(2f * jumpHeight / (rb.gravityScale * 9.81f));
        float jumpInitialVelocity = (jumpHeight + 1f + 0.5f * rb.gravityScale * 9.81f * timeToMaxHeight * timeToMaxHeight) / timeToMaxHeight;
        if (!isInQuicksand) {
            if (horizontalInput < 0) {
                targetVelocity = new Vector2(maxSpeedLeft, rb.velocity.y);
            } else if (horizontalInput > 0) {
                targetVelocity = new Vector2(maxSpeedRight, rb.velocity.y);
            } else if (!isOnGround) {
                targetVelocity = new Vector2(0, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (isOnGround && jumpButton) {
                rb.velocity = new Vector2(rb.velocity.x, jumpInitialVelocity);
                isOnGround = false;
                state = State.jumping;
            } else if (isOnGround && horizontalInput != 0) {
                state = State.running;
            } else if (rb.velocity.y < -0.1f) {
                state = State.falling;
            } else
                state = State.idle;

            //variable jump height
            if (!jumpButton && rb.velocity.y > 0.5f && jumpReleaseActive)
                targetVelocity.y = earlyJumpReleaseYVelocity;
            //smooths out player velocity change
            if (isOnGround)
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothTime);
            else
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, movementSmoothTime / airControl);
        }
        //if in quicksand
        else {
            rb.velocity = new Vector2(0, -0.2f);
        }
        //change player facing direction
        if (horizontalInput < 0) {
            //sprite.flipX = true;
            transform.localScale = new Vector3(-initialScale, initialScale, initialScale);
        } else if (horizontalInput > 0) { 
            //sprite.flipX = false;
            transform.localScale = new Vector3(initialScale, initialScale, initialScale);
        }
    }
	public void ToggleMovementControl(bool toggle) {
		movementAllowed = toggle;
    }
	#endregion

	#region EnvironmentalDetectionFunctions

    /**
     * Detects what surface the player is currently standing on
     */
	void DetectBottomSurface() {
        //detects standing on ground
		RaycastHit2D groundHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, groundDetectRadius, groundLayer);

		isOnGround = groundHit.collider != null;

        //detects standing on quicksand
        RaycastHit2D sandHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, groundDetectRadius, sandLayer);
        isInQuicksand = sandHit.collider != null;
    }

    /**
     * Drawing Gizmos in edit mode to display the raycast for detecting ground and jump height
     */
    [ExecuteInEditMode]
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y - groundDetectRadius));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + jumpHeight));
    }
    #endregion

    /**
     * AssignState - assigns player animation based on current player movement state
     *     works in conjunction with the state variable and enum State
     */
    void AssignState() {
        if (rb.velocity.y > 0.5f)
            state = State.jumping;
        else if (rb.velocity.y < -0.5f)
            state = State.falling;
        else if (Mathf.Abs(horizontalInput) > 0.5f)
            state = State.running;
        else
            state = State.idle;
    }

    /**
     * Teleports the player to the last ground position that the player stood on
     * Used in: teleports player back to previous position on taking damage
     */
    public void OnDamage() {
        transform.position = new Vector2(PlayerData.checkpoint[0], PlayerData.checkpoint[1]);
        rb.velocity = Vector2.zero;
	}

    public void Die() {
        SceneChanger.GoToSpaceship();
    }

    IEnumerator InvincibilityFrames() {
        if (invulnerable == false) { 
            invulnerable = true;
            yield return new WaitForSeconds(invincibilityDuration);
            invulnerable = false;
        }
    }
    public bool isInvulnerable() { return invulnerable; }

    public void TakeDamage() {
        if (!invulnerable) {
            StartCoroutine(InvincibilityFrames());
            PlayerData.currHealth -= 1;
            ui.UpdateHealth();
            //if (PlayerData.currHealth > 0) {
                OnDamage();
            //} else
                //Die();
        }
    }

}
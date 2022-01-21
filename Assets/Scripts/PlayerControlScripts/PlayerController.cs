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
    SpriteRenderer sprite;
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
    private bool movementAllowed = true;
    float initialScale;

    /// New
	public float acceleration = 5f;
    public float decceleration = 5f;
    public float velPower = 2;


    public float frictionAmount;

    [Header("Jump")]
    public float jumpForce;
    [Range(0, 1)]
    public float jumpCutMultiplier;
    [Space(10)]
    public float jumpCoyoteTime;
    private float lastGroundedTime;
    public float jumpBufferTime;
    private float lastJumpTime;
    [Space(10)]
    public float fallGravityMultiplier;
    private float gravityScale;
    [Space(10)]
    private bool isJumping;
    /// EndNew

    [Header("Vertical:")]
    bool jumpButton;
    [HideInInspector] public bool isOnGround;
    [HideInInspector] public bool isInQuicksand;
    [Range(0, 30f)] [SerializeField] float jumpHeight = 2f;

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
    public GrapplingGun grapple;

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
            //print("Checkpoint load: " + PlayerData.checkpoint[0] + ", " + PlayerData.checkpoint[1]);
            transform.position = new Vector2(PlayerData.checkpoint[0], PlayerData.checkpoint[1]);
		}
        gravityScale = rb.gravityScale;
    }

    void Update() {
        if (Input.GetButtonDown("SlowMo")) ToggleSlowMo(true);
        else if (Input.GetButtonUp("SlowMo")) ToggleSlowMo(false);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) { OnJump(); jumpButton = true; }
        else if (Input.GetButtonUp("Jump")) { OnJumpUp(); jumpButton = false; }
		DetectBottomSurface();

        if (isOnGround) lastGroundedTime = jumpCoyoteTime;
        if (rb.velocity.y < 0) isJumping = false;

        if (lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping) //checks if was last grounded within coyoteTime and that jump has been pressed within bufferTime
        {
            Jump();
        }
        #region Timer
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
        #endregion

        animator.SetInteger("state", (int)state);

        sprite.color = (invulnerable) ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
    }

    void FixedUpdate() {
        //limit downward velocity
        if (rb.velocity.y < maxDownVelocity) rb.velocity = new Vector2(rb.velocity.x, maxDownVelocity);
        //movement control
        if (movementAllowed) {
            MovePlayer();

            #region Jump Gravity
            if (rb.velocity.y < 0) {
                rb.gravityScale = gravityScale * fallGravityMultiplier;
            } else {
                rb.gravityScale = gravityScale;
            }
			#endregion
		}


	}

    /**
     * Player's movement controller
     * Includes: vertical/horizontal movement
     */
    void MovePlayer() {

        if (!movementAllowed) {
            return;
        }

		if (!isInQuicksand) {
            #region Run
            float targetSpeed;
            if (horizontalInput > 0) targetSpeed = horizontalInput * maxSpeedRight;
            else if (horizontalInput == 0) targetSpeed = 0f;
            else targetSpeed = horizontalInput * -maxSpeedLeft;
            float speedDif = targetSpeed - rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            //checks if current speed doesn't exceed max speed, if it does just preserve the previous velocity
            if (!(targetSpeed != 0 && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Sign(targetSpeed) != Mathf.Sign(speedDif))) {
                if (isOnGround) rb.AddForce(movement * Vector2.right);
                else rb.AddForce(movement * Vector2.right * airControl);
            }
            
            #endregion
            #region Friction
            if (lastGroundedTime > 0 && Mathf.Abs(horizontalInput) < 0.01f) {
                float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
                amount *= Mathf.Sign(rb.velocity.x);
                rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }
            #endregion
            if (isOnGround && horizontalInput != 0) {
				state = State.running;
			} else if (rb.velocity.y < -0.1f) {
				state = State.falling;
			} else
				state = State.idle;
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

	private void Jump() {
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true;
		isOnGround = false;
		state = State.jumping;
	}

	public void ToggleMovementControl(bool toggle) {
		movementAllowed = toggle;
    }

    public void OnJump() {
        lastJumpTime = jumpBufferTime;
    }

    public void OnJumpUp() {
        if (rb.velocity.y > 0 && isJumping) {
            rb.AddForce(Vector2.down * rb.velocity.y * jumpCutMultiplier, ForceMode2D.Impulse);
        }
    }

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
            if (grapple) grapple.DisableGrapple();
            StartCoroutine(InvincibilityFrames());
            PlayerData.currHealth -= 1;
            ui.UpdateHealth();
            //if (PlayerData.currHealth > 0) {
                OnDamage();
            //} else
                //Die();
        }
    }

    void ToggleSlowMo(bool toggle) {
        if (toggle) Time.timeScale = 0.5f;
        else Time.timeScale = 1.0f;
    }

}
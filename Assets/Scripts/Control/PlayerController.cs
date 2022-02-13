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
    Collider2D coll;
    SpriteRenderer sprite;
    PlayerUI ui;
    enum State { idle, running, jumping, falling, pushing, hurt }; //animation states, decides interactions
    State state;


    [Header("Animation")]
    Animator animator;
    
    [Header("Horizontal:")]
    float horizontalInput;
    private bool movementAllowed = true;




    //scene interactions
    bool invulnerable;
    [Range(0f, 5f)] [SerializeField] float invincibilityDuration = 0f;
    public GrapplingGun grapple;


    /// <summary>
	/// //////
	/// 
	/// </summary>
    PlayerMovement mover;



    void Start() {
        mover = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        state = State.idle;
        mover.rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();

        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>();
        mover.groundLayer = LayerMask.GetMask("Ground");
        mover.sandLayer = LayerMask.GetMask("Sand");


        if (PlayerData.checkpoint == null)
            PlayerData.checkpoint = new float[2] { transform.position.x, transform.position.y };  //initial checkpoint is set to initial position
        else {
            //print("Checkpoint load: " + PlayerData.checkpoint[0] + ", " + PlayerData.checkpoint[1]);
            transform.position = new Vector2(PlayerData.checkpoint[0], PlayerData.checkpoint[1]);
		}
        mover.gravityScale = mover.rb.gravityScale;
    }

    void Update() {
        if (Input.GetButtonDown("SlowMo")) ToggleSlowMo(true);
        else if (Input.GetButtonUp("SlowMo")) ToggleSlowMo(false);

        if (Input.GetButtonDown("Jump")) { mover.SetJumpBuffer(); }
        else if (Input.GetButtonUp("Jump")) { ApplyJumpCut(); }
		DetectBottomSurface();

        if (mover.isOnGround) mover.lastGroundedTime = mover.jumpCoyoteTime;
        if (mover.rb.velocity.y < 0) mover.isJumping = false;

        if (mover.lastGroundedTime > 0 && mover.GetLastJumpTime() > 0 && !mover.isJumping) //checks if was last grounded within coyoteTime and that jump has been pressed within bufferTime
        {
            Jump();
        }
        #region Timer
        mover.lastGroundedTime -= Time.deltaTime;

        #endregion

        animator.SetInteger("state", (int)state);

        sprite.color = (invulnerable) ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
    }

    void FixedUpdate() {
        //limit downward velocity
        if (mover.rb.velocity.y < mover.maxDownVelocity) mover.rb.velocity = new Vector2(mover.rb.velocity.x, mover.maxDownVelocity);
        //movement control
        if (movementAllowed) {
			MovePlayer();

			#region Jump Gravity
			if (Falling()) {
                mover.rb.gravityScale = mover.gravityScale * mover.fallGravityMultiplier;
			} else {
                mover.rb.gravityScale = mover.gravityScale;
			}
			#endregion
		}

	}

	private bool Falling() {
		return mover.rb.velocity.y < 0;
	}

	/**
     * Player's movement controller
     * Includes: vertical/horizontal movement
     */
	void MovePlayer() {
		if (!movementAllowed) {
			return;
		}

		horizontalInput = Input.GetAxisRaw("Horizontal");
		SetFacingDirection(horizontalInput);

		Vector2 relativeVelocity = GetRelativeVelocityToGround();

		if (mover.isInQuicksand) {
            mover.rb.velocity = new Vector2(0, -0.2f);
			return;
		}

        #region Run
        float targetSpeed;
        if (horizontalInput > 0) targetSpeed = horizontalInput * mover.maxSpeed;
        else if (horizontalInput == 0) targetSpeed = 0f;
        else targetSpeed = horizontalInput * mover.maxSpeed;
        float speedDif = targetSpeed - relativeVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? mover.acceleration : mover.decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, mover.velPower) * Mathf.Sign(speedDif);

        //checks if current speed doesn't exceed max speed, if it does just preserve the previous velocity
        if (!(targetSpeed != 0 && Mathf.Sign(relativeVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Sign(targetSpeed) != Mathf.Sign(speedDif))) {
            if (mover.isOnGround) mover.rb.AddForce(movement * Vector2.right);
            else mover.rb.AddForce(movement * Vector2.right * mover.airControl);
        }

        #endregion
        #region Friction
        if (mover.lastGroundedTime > 0 && Mathf.Abs(horizontalInput) < 0.01f) {
			ApplyFriction(relativeVelocity);
		}
		#endregion
		if (mover.isOnGround && horizontalInput != 0) {
			state = State.running;
		} else if (relativeVelocity.y < -0.1f) {
			state = State.falling;
		} else
			state = State.idle;

	}

	private Vector2 GetRelativeVelocityToGround() {
		return (transform.parent != null && mover.groundHitRB != null) ? mover.rb.velocity - mover.groundHitRB.velocity : mover.rb.velocity;
	}

	private void SetFacingDirection(float horizontalInput) {
		if (horizontalInput < 0) {
			FaceLeft();
		} else if (horizontalInput > 0) {
			FaceRight();
		}
	}

	private void FaceRight() {
		transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
	}

	private void FaceLeft() {
		transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
	}

	private void ApplyFriction(Vector2 relativeVelocity) {
		float amount = Mathf.Min(Mathf.Abs(relativeVelocity.x), Mathf.Abs(mover.frictionAmount));
		amount *= Mathf.Sign(relativeVelocity.x);
        mover.rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
	}

	private void Jump() {
        mover.rb.AddForce(Vector2.up * mover.jumpForce, ForceMode2D.Impulse);
        mover.isJumping = true;
        mover.isOnGround = false;
		state = State.jumping;
	}

	public void ToggleMovementControl(bool toggle) {
		movementAllowed = toggle;
    }



    public void ApplyJumpCut() {
        if (mover.rb.velocity.y > 0 && mover.isJumping) {
            mover.rb.AddForce(Vector2.down * mover.rb.velocity.y * mover.jumpCutMultiplier, ForceMode2D.Impulse);
        }
    }

    #region EnvironmentalDetectionFunctions

    /**
     * Detects what surface the player is currently standing on
     */
    void DetectBottomSurface() {
		//detects standing on ground
		RaycastHit2D groundHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, mover.groundDetectRadius, mover.groundLayer);

		mover.isOnGround = groundHit.collider != null;
		if (mover.isOnGround) { mover.groundHitRB = groundHit.collider.GetComponent<Rigidbody2D>(); }

		//detects standing on quicksand
		RaycastHit2D sandHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, mover.groundDetectRadius, mover.sandLayer);
        mover.isInQuicksand = sandHit.collider != null;
    }


    #endregion



    /**
     * Teleports the player to the last ground position that the player stood on
     * Used in: teleports player back to previous position on taking damage
     */
    public void OnDamage() {
        transform.position = new Vector2(PlayerData.checkpoint[0], PlayerData.checkpoint[1]);
        mover.rb.velocity = Vector2.zero;
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
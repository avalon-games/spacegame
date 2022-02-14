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
    SpriteRenderer sprite;
    PlayerUI ui;
    enum State { idle, running, jumping, falling, pushing, hurt };
    State state;

    Animator animator;
    
    float horizontalInput;
    private bool movementAllowed = true;

    bool invulnerable;
    [Range(0f, 5f)] [SerializeField] float invincibilityDuration = 0f;
    public GrapplingGun grapple;

    PlayerMovement mover;


	private void Awake() {
        mover = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>();
    }
	void Start() {
        state = State.idle;

        if (PlayerData.checkpoint == null)
			SetCheckpointToStart();
		else {
			SpawnAtCheckpoint();
		}
    }

	private void SpawnAtCheckpoint() {
		transform.position = new Vector2(PlayerData.checkpoint[0], PlayerData.checkpoint[1]);
	}

	private void SetCheckpointToStart() {
		PlayerData.checkpoint = new float[2] { transform.position.x, transform.position.y };
	}

	void Update() {
        if (Input.GetButtonDown("SlowMo")) ToggleSlowMo(true);
        else if (Input.GetButtonUp("SlowMo")) ToggleSlowMo(false);

        if (Input.GetButtonDown("Jump")) { mover.SetJumpBuffer(); }
        else if (Input.GetButtonUp("Jump")) { mover.ApplyJumpCut(); }

        animator.SetInteger("state", (int)state);

        sprite.color = (invulnerable) ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
    }

    void FixedUpdate() {
        //movement control
        if (movementAllowed) {
			MovePlayer();
		}
	}

	void MovePlayer() {
		if (!movementAllowed) {
			return;
		}

		horizontalInput = Input.GetAxisRaw("Horizontal");
		SetFacingDirection(horizontalInput);

		Vector2 relativeVelocity = mover.GetRelativeVelocityToGround();

		if (mover.isInQuicksand) {
            mover.rb.velocity = new Vector2(0, -0.2f);
			return;
		}

        #region Run
        mover.SetToRun(horizontalInput);
        float speedDif = mover.targetSpeed - relativeVelocity.x;
        float accelRate = (Mathf.Abs(mover.targetSpeed) > 0.01f) ? mover.acceleration : mover.decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, mover.velPower) * Mathf.Sign(speedDif);

        //checks if current speed doesn't exceed max speed, if it does just preserve the previous velocity
        if (!(mover.targetSpeed != 0 && Mathf.Sign(relativeVelocity.x) == Mathf.Sign(mover.targetSpeed) && Mathf.Sign(mover.targetSpeed) != Mathf.Sign(speedDif))) {
            if (mover.isOnGround) mover.rb.AddForce(movement * Vector2.right);
            else mover.rb.AddForce(movement * Vector2.right * mover.airControl);
        }

        #endregion
        #region Friction
        if (mover.lastGroundedTime > 0 && Mathf.Abs(horizontalInput) < 0.01f) {
			mover.ApplyFriction(relativeVelocity);
		}
		#endregion
		if (mover.isOnGround && horizontalInput != 0) {
			state = State.running;
		} else if (relativeVelocity.y < -0.1f) {
			state = State.falling;
		} else
			state = State.idle;

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



	public void ToggleMovementControl(bool toggle) {
		movementAllowed = toggle;
    }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpBufferTime = 0.1f;


    [Range(0, 20f)] public float maxSpeed = 5;
    public float acceleration = 13f;
    public float decceleration = 13f;
    public float velPower = 0.96f;
    public float frictionAmount = 0.22f;
    public float jumpForce = 13;
    [Range(-15f, 0)] public float maxDownVelocity = -10f;
    [Range(0, 1)] public float jumpCutMultiplier = 0.4f;
    public float jumpCoyoteTime = 0.15f;
    public float fallGravityMultiplier = 2;
    [Range(0, 1f)] public float airControl = 0.8f;
    [Range(0, 5f)] public float groundDetectRadius = 1.51f;

    private enum CURRENT_TERRAIN { GRASS, GRAVEL, WOOD_FLOOR, WATER, METAL, STONE };
    [SerializeField]
    private CURRENT_TERRAIN currentTerrain;

    private FMOD.Studio.EventInstance foosteps;



    private float lastJumpTime;
    public float lastGroundedTime;
    public float gravityScale;
    public bool isJumping;
    Vector2 relativeVelocity;
    [HideInInspector] public bool isOnGround;
    [HideInInspector] public bool isInQuicksand;


    [HideInInspector] public Rigidbody2D rb;
    public Rigidbody2D groundHitRB;
    public Collider2D coll;
    Animator anim;
    public LayerMask groundLayer;
    public LayerMask sandLayer;

    public float targetSpeed;


    //animation states
    enum State { idle, running, jumping, falling, pushing, hurt }; 
    State state;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
        sandLayer = LayerMask.GetMask("Sand");
    }

	private void Start() {
        gravityScale = rb.gravityScale;
        state = State.idle;
    }


	public void SetGravityScale() {
		if (IsFalling()) {
			rb.gravityScale = gravityScale * fallGravityMultiplier;
		} else {
			rb.gravityScale = gravityScale;
		}
	}

	public void LimitDownVelocity() {
		if (rb.velocity.y < maxDownVelocity)
            rb.velocity = new Vector2(rb.velocity.x, maxDownVelocity);
	}

	void Update() {
        DetectBottomSurface();
		if (isOnGround) lastGroundedTime = jumpCoyoteTime;
		if (IsFalling()) isJumping = false;

		if (BufferingJump()) {
			JumpBehavior();
		}

		UpdateTimers();

        anim.SetInteger("state", (int)state);
        if (state == State.running) {
            foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps");
            foosteps.setParameterByName("Terrain", terrain);
            foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            foosteps.start();
            foosteps.release();

        } else {

		}
    }

    public void MovementBehavior(float horizontalInput) {
		relativeVelocity = GetRelativeVelocityToGround();

		RunBehavior(horizontalInput);

		if (lastGroundedTime > 0 && Mathf.Abs(horizontalInput) < 0.01f) {
			ApplyFriction(relativeVelocity);
		}
		if (isOnGround && horizontalInput != 0) {
			state = State.running;
		} else if (relativeVelocity.y < -0.1f) {
			state = State.falling;
		} else
			state = State.idle;
	}

	private void RunBehavior(float horizontalInput) {
		SetToRun(horizontalInput);
		float speedDif = targetSpeed - relativeVelocity.x;
		float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
		float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

		if (WithinSpeedLimit(relativeVelocity, speedDif)) {
			if (isOnGround) rb.AddForce(movement * Vector2.right);
			else rb.AddForce(movement * Vector2.right * airControl);
		}
	}

	private bool WithinSpeedLimit(Vector2 relativeVelocity, float speedDif) {
		return targetSpeed == 0 ||
            Mathf.Sign(relativeVelocity.x) != Mathf.Sign(targetSpeed) ||
            Mathf.Sign(targetSpeed) == Mathf.Sign(speedDif);
	}

	public bool IsFalling() {
		return rb.velocity.y < 0;
	}

	private bool BufferingJump() {
		return lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping;
	}

	public void SetJumpBuffer() {
        lastJumpTime = jumpBufferTime;
    }


    private void UpdateTimers() {
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
    }

    private void JumpBehavior() {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumping = true;
        isOnGround = false;
        state = State.jumping;
    }

    void DetectBottomSurface() {
        RaycastHit2D groundHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, groundDetectRadius, groundLayer);

        isOnGround = groundHit.collider != null;
        if (isOnGround) { groundHitRB = groundHit.collider.GetComponent<Rigidbody2D>(); }

        //detects standing on quicksand
        RaycastHit2D sandHit = Physics2D.Raycast(coll.bounds.center, Vector2.down, groundDetectRadius, sandLayer);
        isInQuicksand = sandHit.collider != null;
    }

    public void SetToRun(float direction) {
        targetSpeed = direction * maxSpeed;
    }

    public Vector2 GetRelativeVelocityToGround() {
        return (transform.parent != null && groundHitRB != null) ? rb.velocity - groundHitRB.velocity : rb.velocity;
    }


    public void ApplyJumpCut() {
        if (rb.velocity.y > 0 && isJumping) {
            rb.AddForce(Vector2.down * rb.velocity.y * jumpCutMultiplier, ForceMode2D.Impulse);
        }
    }

    public void ApplyFriction(Vector2 relativeVelocity) {
        float amount = Mathf.Min(Mathf.Abs(relativeVelocity.x), Mathf.Abs(frictionAmount));
        amount *= Mathf.Sign(relativeVelocity.x);
        rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
    }

    public bool QuicksandBehavior() {
		if (!isInQuicksand) {
			return false;
		}
		rb.velocity = new Vector2(0, -0.2f);
		return true;
	}

	//Called by Unity
	void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDetectRadius));
    }


}

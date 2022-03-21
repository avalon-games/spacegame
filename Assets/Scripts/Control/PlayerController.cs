using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    SpriteRenderer sprite;
    PlayerUI ui;
    
    float horizontalInput;
    private bool controlAllowed = true;

    bool invulnerable;
    [Range(0f, 5f)] [SerializeField] float invincibilityDuration = 0f;
    public GrapplingGun grapple;

    PlayerMovement mover;


	private void Awake() {
        mover = GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<PlayerUI>();
    }

	private void Start() {
        if (PlayerData.checkpoint == null) {
            PlayerData.checkpoint = new float[2] { transform.position.x, transform.position.y };
        }
    }

	void Update() {
        if (Input.GetButtonDown("SlowMo")) ToggleSlowMo(true);
        else if (Input.GetButtonUp("SlowMo")) ToggleSlowMo(false);

        if (Input.GetButtonDown("Jump")) { mover.SetJumpBuffer(); }
        else if (Input.GetButtonUp("Jump")) { mover.ApplyJumpCut(); }

        sprite.color = (invulnerable) ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1);
    }

    void FixedUpdate() {
        if (!controlAllowed) {
            return;
        }

        if (grapple.IsAttached()) { return; }
        //if (InteractWithTime()) { return; } 
        //if(InteractWithGrapple()) {return;}
        if (InteractWithMovement()) {return;}
	}

	bool InteractWithMovement() {
		horizontalInput = Input.GetAxisRaw("Horizontal");
		SetFacingDirection(horizontalInput);

        if(mover.QuicksandBehavior()) { return true; }
        mover.LimitDownVelocity();
        mover.SetGravityScale();
        mover.MovementBehavior(horizontalInput);
        return true;
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



	public void ToggleControl(bool toggle) {
		controlAllowed = toggle;
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
        FindObjectOfType<SaveAndLoad>().LoadLevel(1);
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
        if (toggle && ui.GetSlowmoCharge() > 0) {
            ui.ActivateSlowmo();
        } else {
            ui.DeactivateSlowmo();
        }
    }

}
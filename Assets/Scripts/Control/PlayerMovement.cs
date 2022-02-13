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



    private float lastJumpTime;
    public float lastGroundedTime;
    public float gravityScale;
    public bool isJumping;
    [HideInInspector] public bool isOnGround;
    [HideInInspector] public bool isInQuicksand;


    [HideInInspector] public Rigidbody2D rb;
    public Rigidbody2D groundHitRB;
    public LayerMask groundLayer;
    public LayerMask sandLayer;











    void Update() {
        UpdateTimers();
	}

    public void SetJumpBuffer() {
        lastJumpTime = jumpBufferTime;
    }


    private void UpdateTimers() {
        lastJumpTime -= Time.deltaTime;
    }

    public float GetLastJumpTime() { return lastJumpTime; }

    /**
     * Drawing Gizmos in edit mode to display the raycast for detecting ground and jump height
     */
    [ExecuteInEditMode]
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDetectRadius));
    }

}

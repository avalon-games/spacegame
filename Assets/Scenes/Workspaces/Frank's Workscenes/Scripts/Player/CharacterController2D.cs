using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    /* Brackey's 2D Character Controller Script - Modified Slightly */
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] float smoothTime = 0.5f;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;

    const float groundRadius = 0.01f;
    const float ceilingRadius = 0.2f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Vector3 smoothDampVelocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    public void Move(float move, bool jump)
    {
        if (isGrounded)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref smoothDampVelocity, smoothTime);

            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }

        if (isGrounded && jump)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

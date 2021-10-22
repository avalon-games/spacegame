using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl2D : MonoBehaviour
{
    public CharacterController2D controller;

    [SerializeField]
    private float runSpeed = 40f;
    private float horizontalMove = 0f;

    bool jump = false;

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }
}

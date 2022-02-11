using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float jumpBufferTime = 0.1f;
    private float lastJumpTime;

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

}

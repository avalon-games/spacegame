using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDoor : MonoBehaviour
{
    Animator anim;
    Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Player") && anim != null) {
            anim.Play("Open");
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && anim != null) {
            anim.Play("Close");
        }
    }

	//private void OnCollisionEnter2D(Collision2D collision) {
 //       if (collision.gameObject.CompareTag("Player") && anim != null) {
 //           anim.Play("Open");
 //       }
 //   }

	//private void OnCollisionExit2D(Collision2D collision) {
 //       if (collision.gameObject.CompareTag("Player") && anim != null) {
 //           anim.Play("Open");
 //       }
 //   }

    public void DisableCollider() {
        coll.enabled = false;
	}

    public void EnableCollider() {
        coll.enabled = true;
	}
}

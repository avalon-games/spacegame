using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Tutorial: https://www.youtube.com/watch?v=6hp9-mslbzI
 */
public class ArmRotator : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GrapplingGun grapple;


    private void FixedUpdate()
    {
        Vector2 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		if (grapple.IsAttached()) {
            Vector2 anch = grapple.GetAnchorPoint();
            if (anch != Vector2.negativeInfinity)
                difference = grapple.GetAnchorPoint() - (Vector2)transform.position;
		}


		difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 100;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

  //      if (player.transform.localScale.x < 0) {
  //          transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
		//} else {
  //          transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
		//}

	}

}
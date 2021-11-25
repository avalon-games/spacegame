using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private GameObject rayBarrier; //the spike will not drop if player crosses below the ypos of this object
    private float maxRayDistance;
    private bool dropped;

    // Start is called before the first frame update
    void Start()
    {
        maxRayDistance = transform.position.y - rayBarrier.transform.position.y;
        dropped = false;
    }

    private void FixedUpdate()
    {
        if (dropped)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up * maxRayDistance);
        Debug.DrawRay(transform.position, Vector2.down * maxRayDistance);

        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDrop : MonoBehaviour
{
    private Collider2D boxCollider;

    [SerializeField] private GameObject Spikes;

    private void Awake()
    {
        boxCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        foreach (Transform child in Spikes.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}

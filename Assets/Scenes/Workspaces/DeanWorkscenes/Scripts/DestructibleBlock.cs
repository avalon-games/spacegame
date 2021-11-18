using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBlock : MonoBehaviour
{
    Collider2D coll;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBlock() {
        StartCoroutine(DestroyAndWait());
	}

    IEnumerator DestroyAndWait() {
        yield return new WaitForSeconds(2f);
        coll.enabled = false;
        sprite.enabled = false; //make transparent
        yield return new WaitForSeconds(5f);
        coll.enabled = true;
        sprite.enabled = true;

	}
}

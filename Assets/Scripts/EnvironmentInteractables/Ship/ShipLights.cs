using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLights : MonoBehaviour
{
    List<SpriteRenderer> sprites;
    // Start is called before the first frame update
    void Start()
    {
        sprites = new List<SpriteRenderer>();
        foreach(Transform child in transform) {
            sprites.Add(child.gameObject.GetComponent<SpriteRenderer>());
		}
    }

    public void EnableShipSprite(int i) {
        for(int j = 0; j < sprites.Count; j++) {
            sprites[j].enabled = (j == i) ? true : false;
		}
	}
}

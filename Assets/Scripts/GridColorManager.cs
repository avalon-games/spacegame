using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/** removes sprite color tint on playmode
 * 
 */
public class GridColorManager : MonoBehaviour
{
    List<Tilemap> tilemaps;
    void Start() {
        tilemaps = new List<Tilemap>();
        foreach (Transform child in transform) {
            tilemaps.Add(child.GetComponent<Tilemap>());
        }
        foreach(Tilemap t in tilemaps) {
            t.color = Color.white;
		}
    }
}

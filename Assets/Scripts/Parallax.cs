using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Allows the background graphics to have parallax effect
 * 
 * Tutorial used: https://youtu.be/zit45k6CUMk
 */
public class Parallax : MonoBehaviour
{
    float length, startpos;
    [SerializeField] GameObject cam;
    [SerializeField] float parallaxEffect; 

    void Start() {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate() {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        //parallax scrolling
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        //repeat pattern at end of sprite - infinite scrolling
        if (temp > startpos + length) startpos += (length-2);
        else if (temp < startpos - length) startpos -= (length-2);
    }
}

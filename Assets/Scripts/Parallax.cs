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
    float length, startPosX, startPosY;
    [SerializeField] GameObject cam;
    [SerializeField] float parallaxEffect;
    [SerializeField] Vector2 offset;

    void Start() {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate() {
        float tempX = (cam.transform.position.x * (1 - parallaxEffect));
        float distX = (cam.transform.position.x * parallaxEffect);
        float distY = (cam.transform.position.y * parallaxEffect);
        //parallax scrolling
        transform.position = new Vector3(startPosX + distX + offset.x, startPosY + distY + offset.y, transform.position.z);
        //repeat pattern at end of sprite - infinite scrolling
        if (tempX > startPosX + length) startPosX += (length-2);
        else if (tempX < startPosX - length) startPosX -= (length-2);
    }
}

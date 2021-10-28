using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float length, startpos;
    [SerializeField] GameObject cam;
    [SerializeField] float parallaxEffect;
    //Vector3 v = Vector3.zero;
    //[SerializeField] float smoothing = 5f;

    void Start() {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update() {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        //Vector3 targetPosition = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref v, smoothing);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}

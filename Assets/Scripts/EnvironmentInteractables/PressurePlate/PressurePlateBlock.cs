using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateBlock : MonoBehaviour
{
    private BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        collider.enabled = true;
        StartCoroutine("FadeInColour");
    }

    public void Deactivate()
    {
        collider.enabled = false;
        StartCoroutine("FadeOutColour");
    }

    public IEnumerator FadeInColour()
    {
        float startTime = Time.time;
        float startValue = 0.5f;
        float endValue = 1f;
        float totalTime = 1f;
        Color currentColor = GetComponent<SpriteRenderer>().color;

        while (Time.time < startTime + totalTime)
        {
            float newAlpha = Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
            GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b);
    }

    public IEnumerator FadeOutColour()
    {
        float startTime = Time.time;
        float startValue = 1f;
        float endValue = 0.5f;
        float totalTime = 1f;
        Color currentColor = GetComponent<SpriteRenderer>().color;

        while (Time.time < startTime + totalTime)
        {
            float newAlpha = Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
            GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f);
    }
}

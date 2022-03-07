using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateBlock : MonoBehaviour
{
    Collider2D collider;
    [SerializeField] bool enableMovementToggle; //for moving platforms
    [SerializeField] bool enableColliderToggle;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public void Activate()
    {
        if (enableColliderToggle) collider.enabled = true;
        if (enableMovementToggle && GetComponent<LinearMovingPlatform>()) {
            GetComponent<LinearMovingPlatform>().enabled = true;
        }
        StartCoroutine("FadeInColour");
        
    }

    public void Deactivate()
    {
        if (enableColliderToggle) { collider.enabled = false; }
        if (enableMovementToggle && GetComponent<LinearMovingPlatform>()) {
            GetComponent<LinearMovingPlatform>().enabled = false;
        }
        StartCoroutine("FadeOutColour");
        
    }

    public IEnumerator FadeInColour()
    {
        float startTime = Time.time;
        float startValue = 0.2f;
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
        float endValue = 0.2f;
        float totalTime = 1f;
        Color currentColor = GetComponent<SpriteRenderer>().color;

        while (Time.time < startTime + totalTime)
        {
            float newAlpha = Mathf.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
            GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.2f);
    }
}

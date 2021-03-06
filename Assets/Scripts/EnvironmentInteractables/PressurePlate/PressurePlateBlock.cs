using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PressurePlateBlock : MonoBehaviour
{
    [SerializeField] bool enableMovementToggle; //for moving platforms
    [SerializeField] bool enableColliderToggle;

    Collider2D collider;
    LinearMovingPlatform movingPlatform;
    Rigidbody2D rb;
    SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        movingPlatform = GetComponent<LinearMovingPlatform>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Activate()
    {
        if (enableColliderToggle) collider.enabled = true;
        if (enableMovementToggle && movingPlatform) {
            movingPlatform.enabled = true;
        }
        StartCoroutine("FadeInColour");
        
    }

    public void Deactivate()
    {
        if (enableColliderToggle) { collider.enabled = false; }
        if (enableMovementToggle && movingPlatform) {
            movingPlatform.enabled = false;
        }

        StartCoroutine("FadeOutColour");
        rb.velocity = Vector2.zero;
        
    }

    public IEnumerator FadeInColour()
    {
        float startTime = Time.time;
		
        float startValue = 0.2f;
        if (sprite.color.a != 0.2f) { startValue = sprite.color.a; }

        float endValue = 1f;
        float totalTime = 1f;
        Color currentColor = sprite.color;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSlot : MonoBehaviour
{
    [SerializeField]
    private Hotbar hotbar;

    private GameObject newItem;
    private GameObject currentItem;
    private GameObject hexMask;

    private bool flyInCoroutineActive;
    private bool flyOutCoroutineActive;

    private void Start()
    {
        hexMask = GameObject.Find("BackHex");
        flyInCoroutineActive = false;
        flyOutCoroutineActive = false;
    }

    public void SetNewItem(GameObject newItem)
    {
        if (flyOutCoroutineActive)
        {
            flyOutCoroutineActive = false;
            StopCoroutine("ItemFlyOut");
            Destroy(currentItem);
        }

        if (this.newItem)
        {
            currentItem = this.newItem;
        }
        this.newItem = Instantiate(newItem, hexMask.transform);
        this.newItem.transform.localPosition = new Vector2(-10f, 0);

        if (flyInCoroutineActive)
        {
            StopCoroutine("ItemFlyIn");
        }

        StartCoroutine("ItemFlyIn", this.newItem);

        if (currentItem)
        {
            StartCoroutine("ItemFlyOut", currentItem);
        }
    }

    IEnumerator ItemFlyIn(GameObject newItem)
    {
        flyInCoroutineActive = true;

        float startTime = Time.time;
        Vector2 startValue = new Vector2(-50f, 0);
        Vector2 endValue = Vector2.zero;
        float totalTime = 0.5f;

        while (Time.time < startTime + totalTime)
        {
            newItem.transform.localPosition = Vector2.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
            yield return null;
        }

        flyInCoroutineActive = false;
        newItem.transform.localPosition = endValue;
    }

    IEnumerator ItemFlyOut(GameObject currentItem)
    {
        flyOutCoroutineActive = true;
        float startTime = Time.time;
        Vector2 startValue = Vector2.zero;
        Vector2 endValue = new Vector2(50f, 0);
        float totalTime = 0.5f;

        while (Time.time < startTime + totalTime)
        {
            currentItem.transform.localPosition = Vector2.Lerp(startValue, endValue, (Time.time - startTime) / totalTime);
            yield return null;
        }

        currentItem.transform.localPosition = endValue;
        flyOutCoroutineActive = false;
        Destroy(currentItem);
    }
}

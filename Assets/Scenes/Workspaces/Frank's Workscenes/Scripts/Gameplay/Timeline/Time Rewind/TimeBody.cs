using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    private bool isRecording = false;

    private float recordRate = 15f;

    private float recordDuration = 120f;

    private List<Vector2> pointsInTime;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        pointsInTime = new List<Vector2>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isRecording)
        {
            Record();
        }
    }

    private void Record()
    {
        recordRate = ++recordRate % 15;
        if (recordRate != 0)
        {
            return;
        }

        if (pointsInTime.Count > Mathf.Round(recordDuration / Time.fixedDeltaTime))
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Add(transform.position);
    }
    
    public void StartTimeLoop()
    {
        StartCoroutine("Rewind");
    }

    private IEnumerator Rewind()
    {
        rb.isKinematic = true;

        for (int i = pointsInTime.Count - 1; i > 0; i--)
        {
            float startTime = Time.time;
            float duration = 0.25f;

            while (Time.time < startTime + duration)
            {
                transform.position = Vector2.Lerp(pointsInTime[i], pointsInTime[i - 1], (Time.time - startTime) / duration);
                yield return new WaitForEndOfFrame();
            }

            transform.position = pointsInTime[i - 1];
            pointsInTime.RemoveAt(i);
        }
        yield return false;
    }

    private void RewindObject(Vector2 startPos, Vector2 endPos)
    {
        float startTime = Time.time;
        float duration = 1f;

        while (Time.time < startTime + duration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, (Time.time - startTime) / duration);
        }

        transform.position = endPos;
    }

    public void StartRecording()
    {
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }
}

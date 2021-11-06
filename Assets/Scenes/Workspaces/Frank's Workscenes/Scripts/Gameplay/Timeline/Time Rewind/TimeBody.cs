using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeBody : MonoBehaviour
{
    private bool isRecording = false;

    private float recordRate = 15f;

    private float recordDuration = 120f;

    protected List<PointInTime> pointsInTime;

    protected Rigidbody2D rb;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isRecording)
        {
            Record();
        }
    }

    protected virtual void Record()
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

        pointsInTime.Add(new PointInTime(transform.position, transform.rotation, transform.GetComponent<SpriteRenderer>().flipX));
    }
    
    public void StartTimeLoop()
    {
        StartCoroutine("Rewind");
    }

    protected virtual IEnumerator Rewind()
    {
        rb.isKinematic = true;

        for (int i = pointsInTime.Count - 1; i > 0; i--)
        {
            float startTime = Time.time;
            float duration = 0.25f;

            while (Time.time < startTime + duration)
            {
                transform.position = Vector2.Lerp(pointsInTime[i].GetPosition(), pointsInTime[i - 1].GetPosition(), (Time.time - startTime) / duration);
                yield return new WaitForEndOfFrame();
            }

            transform.rotation = pointsInTime[i - 1].GetRotation();
            transform.position = pointsInTime[i - 1].GetPosition();
            transform.GetComponent<SpriteRenderer>().flipX = pointsInTime[i - 1].GetFaceRight();
            pointsInTime.RemoveAt(i);
        }
        OnTimeLoopEnd();
        yield return false;
    }
    /*
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
    */
    public void StartRecording()
    {
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    protected abstract void OnTimeLoopEnd();
}

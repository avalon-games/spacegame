using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RampManager : MonoBehaviour
{
    [SerializeField] private GameObject rampButton;
    private bool buttonActive = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && buttonActive)
        {
            rampButton.GetComponent<Button>().interactable = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && buttonActive)
        {
            rampButton.GetComponent<Button>().interactable = false;
        }
    }

    public void OnRampButtonClicked()
    {
        buttonActive = false;
        rampButton.GetComponent<Button>().interactable = false;
        StartCoroutine("RaiseRamp");
    }

    private IEnumerator RaiseRamp()
    {
        float startTime = Time.time;
        float duration = 3.0f;
        Vector2 startPos = transform.position;
        Vector2 endPos = new Vector2(startPos.x, startPos.y + 6f);

        while (Time.time < startTime + duration)
        {
            transform.position = Vector2.Lerp(startPos, endPos, (Time.time - startTime) / duration);
            yield return null;
        }

        transform.position = endPos;
    }
}

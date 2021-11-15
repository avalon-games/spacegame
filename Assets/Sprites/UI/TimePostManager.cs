using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePostManager : MonoBehaviour
{
    private TimeBody[] timeBodies;

    [SerializeField] private GameObject timeButton;
    [SerializeField] private GameObject timeLoopButton;
    private bool buttonActive = true;

    private void Start()
    {
        timeBodies = FindObjectsOfType<TimeBody>();
    }

    public void OnTimeLoopClicked()
    {
        timeButton.GetComponent<Button>().interactable = false;
        timeLoopButton.GetComponent<Button>().interactable = true;
        foreach (TimeBody timeBody in timeBodies)
        {
            timeBody.StartRecording();
        }
    }

    public void OnTimeLoopStartClicked()
    {
        buttonActive = false;
        timeLoopButton.GetComponent<Button>().interactable = false;
        foreach (TimeBody timeBody in timeBodies)
        {
            timeBody.StopRecording();
            timeBody.StartTimeLoop();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && buttonActive)
        {
            timeButton.GetComponent<Button>().interactable = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && buttonActive)
        {
            timeButton.GetComponent<Button>().interactable = false;
        }
    }
}

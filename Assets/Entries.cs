using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Entries : MonoBehaviour
{
    private static int activeEntry;

    [SerializeField]
    private TextMeshProUGUI headerTMP;
    [SerializeField]
    private TextMeshProUGUI bodyTMP;

    public Button previousEntry;
    public Button nextEntry;

    private void OnEnable()
    {
        SetText();

        previousEntry = GameObject.Find("PreviousEntry").GetComponent<Button>();
        nextEntry = GameObject.Find("NextEntry").GetComponent<Button>();

        if (PlayerData.entries.Count <= 1)
        {
            previousEntry.enabled = false;
            nextEntry.enabled = false;
            return;
        }
        else if (activeEntry == 0)
        {
            nextEntry.enabled = true;
            previousEntry.enabled = false;
            return;
        }
        else if (activeEntry == PlayerData.entries.Count - 1)
        {
            nextEntry.enabled = false;
            previousEntry.enabled = true;
        }
        else
        {
            nextEntry.enabled = true;
            previousEntry.enabled = true;
        }
    }

    public void OnNextEntrySelected()
    {
        if (activeEntry >= PlayerData.entries.Count - 1)
        {
            return;
        }

        activeEntry++;
        SetText();
        previousEntry.enabled = true;

        if (activeEntry == PlayerData.entries.Count - 1)
        {
            nextEntry.enabled = false;
        }
    }

    public void OnPreviousEntrySelected()
    {
        if (activeEntry <= 0)
        {
            return;
        }

        activeEntry--;
        SetText();
        nextEntry.enabled = true;

        if (activeEntry == 0)
        {
            previousEntry.enabled = false;
            return;
        }
    }

    private void SetText()
    {
        if (PlayerData.entries.Count == 0)
        {
            return;
        }

        headerTMP.text = PlayerData.entries[activeEntry].GetHeaderText();
        bodyTMP.text = PlayerData.entries[activeEntry].GetBodyText();
    }
}

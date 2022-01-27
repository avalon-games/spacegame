using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JournalEntry : MonoBehaviour
{
    private string headerText;
    private string bodyText;

    [SerializeField]
    private string headerTextAssetName;
    [SerializeField]
    private string bodyTextAssetName;

    private bool activable;
    private bool added;

    void Start()
    {
        TextAsset headerAsset = Resources.Load("Entries/" + headerTextAssetName) as TextAsset;
        TextAsset bodyAsset = Resources.Load("Entries/" + bodyTextAssetName) as TextAsset;

        headerText = headerAsset.ToString();
        bodyText = bodyAsset.ToString();

        activable = false;
        added = false;

        Debug.Log(headerAsset);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && activable && !added)
        {
            Debug.Log("Add " + headerText);
            AddEntry();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        activable = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        activable = false;
    }

    public void AddEntry()
    {
        PlayerData.entries.Add(this);
    }

    public string GetHeaderText()
    {
        return headerText;
    }

    public string GetBodyText()
    {
        return bodyText;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Button diagnosticButton;

    [SerializeField]
    private GameObject General;
    [SerializeField]
    private GameObject Diagnostic;
    [SerializeField]
    private GameObject Map;
    [SerializeField]
    private GameObject Journal;
    // [SerializeField]
    private GameObject Quit;

    private void OnEnable()
    {
        diagnosticButton.Select();
        SetActive(Diagnostic.name);
    }

    public void OnGeneralSelected()
    {
        SetActive(General.name);
    }

    public void OnDiagnosticsSelected()
    {
        SetActive(Diagnostic.name);
    }

    public void OnMapSelected()
    {
        SetActive(Map.name);
    }

    public void OnJournalSelected()
    {
        SetActive(Journal.name);
    }

    private void SetActive(string name)
    {
        General.SetActive(name == General.name);
        Diagnostic.SetActive(name == Diagnostic.name);
        Map.SetActive(name == Map.name);
        Journal.SetActive(name == Journal.name);
    }
}

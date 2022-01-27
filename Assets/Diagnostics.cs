using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Diagnostics : MonoBehaviour
{
    [SerializeField]
    private GameObject spacesuit;
    [SerializeField]
    private GameObject helmet;
    [SerializeField]
    private GameObject body;
    [SerializeField]
    private GameObject boots;
    [SerializeField]
    private GameObject gloves;
    [SerializeField]
    private GameObject leggings;
    [SerializeField]
    private GameObject health;
    [SerializeField]
    private GameObject oxygen;

    void OnEnable()
    {
        Debug.Log("Enable");
        UpdateDiagnostics();
        UpdateHealth();
        UpdateOxygen();
        UpdateSuit();
        UpdateHelmet();
        UpdateBody();
        UpdateBoots();
        UpdateGloves();
        UpdateLeggings();
    }

    private void UpdateDiagnostics()
    {
        GetComponent<Animator>().Play("ImageFillIn");
    }

    private void UpdateHealth()
    {
        health.GetComponent<TextMeshProUGUI>().text = "Health: " + PlayerData.currHealth * 100 + "%";
    }

    private void UpdateOxygen()
    {
        oxygen.GetComponent<TextMeshProUGUI>().text = "Oxygen: " + PlayerData.currOxygen + "%";
    }

    private void UpdateSuit()
    {
        spacesuit.GetComponent<Animator>().Play("ImageFillIn");
    }

    private void UpdateHelmet()
    {
        helmet.GetComponent<Animator>().Play("ImageFillIn");
        helmet.transform.Find("SolarVisor").gameObject.GetComponent<Animator>().Play("TextFadeIn");
        helmet.transform.Find("HelmetIntegrity").gameObject.GetComponent<Animator>().Play("TextFadeIn");
    }
    private void UpdateBody()
    {
        body.GetComponent<Animator>().Play("ImageFillIn");
        body.transform.Find("Spacesuit").gameObject.GetComponent<Animator>().Play("TextFadeIn");
        body.transform.Find("SuitIntegrity").gameObject.GetComponent<Animator>().Play("TextFadeIn");
    }
    private void UpdateBoots()
    {
        boots.GetComponent<Animator>().Play("ImageFillIn");
        boots.transform.Find("G1-X").gameObject.GetComponent<Animator>().Play("TextFadeIn");
        boots.transform.Find("BootsIntegrity").gameObject.GetComponent<Animator>().Play("TextFadeIn");
    }
    private void UpdateGloves()
    {
        gloves.GetComponent<Animator>().Play("ImageFillIn");
        gloves.transform.Find("Terra").gameObject.GetComponent<Animator>().Play("TextFadeIn");
        gloves.transform.Find("GlovesIntegrity").gameObject.GetComponent<Animator>().Play("TextFadeIn");
    }
    private void UpdateLeggings()
    {
        leggings.GetComponent<Animator>().Play("ImageFillIn");
        leggings.transform.Find("TX").gameObject.GetComponent<Animator>().Play("TextFadeIn");
        leggings.transform.Find("LeggingsIntegrity").gameObject.GetComponent<Animator>().Play("TextFadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        spacesuit.transform.Rotate(new Vector2(0, 1.5f), Space.Self);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HookUI : MonoBehaviour
{
    TextMeshProUGUI hookText;
    public GrapplingGun grapple;
    int prevValue;

    void Start()
    {
        hookText = transform.GetComponentInChildren<TextMeshProUGUI>();
        prevValue = grapple.totalCharge;
    }

    // Update is called once per frame
    void Update()
    {
        if (grapple.totalCharge != prevValue) { 
            hookText.text = grapple.totalCharge.ToString();
            prevValue = grapple.totalCharge;
        }
    }
}

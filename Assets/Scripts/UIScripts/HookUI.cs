using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookUI : MonoBehaviour
{
    Text hookText;
    public GrapplingGun grapple;
    int prevValue;

    void Start()
    {
        hookText = transform.GetComponentInChildren<Text>();
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

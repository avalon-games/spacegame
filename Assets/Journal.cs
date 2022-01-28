using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Animator>().Play("ImageFillIn");
    }
}

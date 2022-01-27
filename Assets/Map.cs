using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private GameObject minimapCamera;

    private void OnEnable()
    {
        GetComponent<Animator>().Play("ImageFillIn");
        minimapCamera.SetActive(true);
    }

    private void OnDisable()
    {
        minimapCamera.SetActive(false);
    }
}

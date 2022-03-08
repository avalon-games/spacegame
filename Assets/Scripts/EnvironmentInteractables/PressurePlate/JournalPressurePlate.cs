using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class JournalPressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private bool textActive;

    private Animation animation;

    private float originalY;
    private float floatStrength = 3f;
    [SerializeField] private string filename;

    // Start is called before the first frame update
    void Start()
    {
        animation = text.GetComponent<Animation>();
        originalY = text.transform.position.y;
        textActive = false;

        SetText();
    }

    private void SetText()
    {
        text.GetComponent<TextMeshProUGUI>().text = File.ReadAllText("Assets/Resources/Entries/" + filename + ".txt");
    }

    // Update is called once per frame
    void Update()
    {
        text.transform.position = new Vector2(text.transform.position.x,
                                        originalY + ((float)Mathf.Sin(Time.time * 2) / floatStrength));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!textActive && collision.gameObject.tag == "Player")
        {
            textActive = true;
            animation.Play("TextAnimation");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (textActive && collision.gameObject.tag == "Player")
        {
            animation.Play("FadeOutAnimation");
            textActive = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject button;
    private bool buttonActive = true;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject block;
    [SerializeField] private GameObject buttonObject;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && buttonActive)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && buttonActive)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void OnButtonClicked()
    {
        buttonObject.GetComponent<SpriteRenderer>().color = Color.blue;
        obstacle.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        obstacle.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        obstacle.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, -5.0f));
        block.GetComponent<Rigidbody2D>().AddForce(new Vector2(1000f, 0f));
    }
}

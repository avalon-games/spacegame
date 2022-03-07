using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PressurePlateTrigger : MonoBehaviour
{
    public List<GameObject> pressureBlocks;

    [SerializeField] private GameObject text;
    private bool textActive;

    private Animation animation;

    private float originalY;
    private Vector2 floatY;
    private float floatStrength = 3f;
    private bool blocksActive;
    [SerializeField] float timeToDeactivate = 5f;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform pressureBlock in transform.parent)
        {
            if (pressureBlock.GetComponent<PressurePlateBlock>())
            {
                pressureBlocks.Add(pressureBlock.gameObject);
            }
        }

        animation = text.GetComponent<Animation>();
        originalY = text.transform.position.y;
        textActive = false;
        blocksActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        text.transform.position = new Vector2(transform.position.x,
                                        originalY + ((float)Mathf.Sin(Time.time * 2) / floatStrength));

        if (textActive && !blocksActive && Input.GetKeyDown(KeyCode.P))
        {
            ActivateBlocks();
        }
    }

    public void ActivateBlocks()
    {
        CancelInvoke();
        Debug.Log("Activate Blocks");
        blocksActive = true;
        foreach (GameObject block in pressureBlocks)
        {
            block.GetComponent<PressurePlateBlock>().Activate();
        }
        Invoke("DeactivateBlocks", timeToDeactivate);
    }

    public void DeactivateBlocks()
    {
        CancelInvoke();
        foreach (GameObject block in pressureBlocks)
        {
            block.GetComponent<PressurePlateBlock>().Deactivate();
        }
        blocksActive = false;
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

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("MechanismHook")) {
            ActivateBlocks();
		}
	}
}

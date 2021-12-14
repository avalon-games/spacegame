using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField]
    private GameObject ActiveSlot;

    public GameObject[] inventoryItems;
    private GameObject currentItem;
    private Color inactiveItem;
    private Color activeItem;

    // Start is called before the first frame update
    void Start()
    {
        inactiveItem = new Color(1, 1, 1, 0.5f);
        activeItem = Color.white;
        inventoryItems = new GameObject[6];
        FillInventory();
    }

    private void FillInventory()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            inventoryItems[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInventoryKey();
    }

    private void CheckInventoryKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (inventoryItems[0].GetComponent<InventorySlot>().IsSlotFilled())
            {
                currentItem = inventoryItems[0].transform.Find("InventoryItem").gameObject;
                HighlightActiveSlot(0);
                ActiveSlot.GetComponent<ActiveSlot>().SetNewItem(currentItem);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (inventoryItems[1].GetComponent<InventorySlot>().IsSlotFilled())
            {
                currentItem = inventoryItems[1].transform.Find("InventoryItem").gameObject;
                HighlightActiveSlot(1);
                ActiveSlot.GetComponent<ActiveSlot>().SetNewItem(currentItem);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (inventoryItems[2].GetComponent<InventorySlot>().IsSlotFilled())
            {
                currentItem = inventoryItems[2].transform.Find("InventoryItem").gameObject;
                HighlightActiveSlot(2);
                ActiveSlot.GetComponent<ActiveSlot>().SetNewItem(currentItem);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (inventoryItems[3].GetComponent<InventorySlot>().IsSlotFilled())
            {
                currentItem = inventoryItems[3].transform.Find("InventoryItem").gameObject;
                HighlightActiveSlot(3);
                ActiveSlot.GetComponent<ActiveSlot>().SetNewItem(currentItem);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            if (inventoryItems[4].GetComponent<InventorySlot>().IsSlotFilled())
            {
                currentItem = inventoryItems[4].transform.Find("InventoryItem").gameObject;
                HighlightActiveSlot(4);
                ActiveSlot.GetComponent<ActiveSlot>().SetNewItem(currentItem);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            if (inventoryItems[5].GetComponent<InventorySlot>().IsSlotFilled())
            {
                currentItem = inventoryItems[5].transform.Find("InventoryItem").gameObject;
                HighlightActiveSlot(5);
                ActiveSlot.GetComponent<ActiveSlot>().SetNewItem(currentItem);
            }
        }
    }

    private void HighlightActiveSlot(int slot)
    {
        inventoryItems[0].GetComponent<Image>().color = (slot == 0 ? activeItem : inactiveItem);
        inventoryItems[1].GetComponent<Image>().color = (slot == 1 ? activeItem : inactiveItem);
        inventoryItems[2].GetComponent<Image>().color = (slot == 2 ? activeItem : inactiveItem);
        inventoryItems[3].GetComponent<Image>().color = (slot == 3 ? activeItem : inactiveItem);
        inventoryItems[4].GetComponent<Image>().color = (slot == 4 ? activeItem : inactiveItem);
        inventoryItems[5].GetComponent<Image>().color = (slot == 5 ? activeItem : inactiveItem);
    }
}

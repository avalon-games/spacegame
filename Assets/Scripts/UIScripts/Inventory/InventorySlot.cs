using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private bool slotFilled;

    // Start is called before the first frame update
    void Start()
    {
        slotFilled = transform.childCount == 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsSlotFilled()
    {
        return slotFilled;
    }
}

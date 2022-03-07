using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeBody : TimeBody
{

    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator Rewind()
    {
        GetComponent<PlayerController>().ToggleControl(false);
        return base.Rewind();
    }

    protected override void OnTimeLoopEnd()
    {
        GetComponent<PlayerController>().ToggleControl(true);
        rb.isKinematic = false;
    }
}

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
        GetComponent<PlayerController>().ToggleMovementControl(false);
        return base.Rewind();
    }

    protected override void OnTimeLoopEnd()
    {
        GetComponent<PlayerController>().ToggleMovementControl(true);
        rb.isKinematic = false;
    }
}

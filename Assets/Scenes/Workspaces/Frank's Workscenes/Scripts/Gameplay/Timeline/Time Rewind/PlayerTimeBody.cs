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
        GetComponent<CharacterController2DAlt>().EnableMovement(false);
        return base.Rewind();
    }

    protected override void OnTimeLoopEnd()
    {
        GetComponent<CharacterController2DAlt>().EnableMovement(true);
        rb.isKinematic = false;
    }
}

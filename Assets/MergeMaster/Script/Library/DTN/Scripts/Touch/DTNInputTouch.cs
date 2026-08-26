using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTNInputTouch : DTNTouch
{
    public DTNInputTouch(int touchId)
    {
        fingerId = touchId;
        TouchRun();
    }

    public override bool OnBegan()
    {
        return base.OnBegan();
    }

    public override bool OnMoved()
    {
        return base.OnMoved();
    }

    public override bool OnEnded()
    {
        return base.OnEnded();
    }
}

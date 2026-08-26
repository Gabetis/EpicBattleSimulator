using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTNInputMouse : DTNTouch
{
    public DTNInputMouse(int touchId)
    {
        fingerId = touchId;
        TouchRun();
    }

    public override bool OnBegan()
    {
        if (Input.GetMouseButtonDown(fingerId))
        {
            phase = DTNTouchPhase.Began;
            return true;
        }
        return false;
    }

    public override bool OnMoved()
    {
        if (Input.GetMouseButton(fingerId))
        {
            phase = DTNTouchPhase.Moved;
            return true;
        }
        return false;
    }

    public override bool OnEnded()
    {
        if (Input.GetMouseButtonUp(fingerId))
        {
            phase = DTNTouchPhase.Ended;
            return true;
        }
        return false;
    }

    public override void TouchPosition()
    {
        position = Input.mousePosition;
    }
}

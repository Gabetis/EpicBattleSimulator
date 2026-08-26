using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DTNTouchPhase
{
    Began = 0,
    Moved = 1,
    Ended = 2
}

public class DTNTouch
{
    public DTNTouch()
    {
        
    }

    public int fingerId;
    public Vector2 position;
    public DTNTouchPhase phase;

    public virtual void TouchRun()
    {
        TouchPosition();

        if(!OnBegan())
            if(!OnMoved())
                OnEnded();
    }

    public virtual bool OnBegan()
    {
        if(Input.touches[fingerId].phase == TouchPhase.Began)
        {
            phase = DTNTouchPhase.Began;
            return true;
        }
        return false;
    }

    public virtual bool OnMoved()
    {
        if (Input.touches[fingerId].phase == TouchPhase.Moved || Input.touches[fingerId].phase == TouchPhase.Stationary)
        {
            phase = DTNTouchPhase.Moved;
            return true;
        }
        return false;
    }

    public virtual bool OnEnded()
    {
        if (Input.touches[fingerId].phase == TouchPhase.Ended || Input.touches[fingerId].phase == TouchPhase.Canceled)
        {
            phase = DTNTouchPhase.Ended;
            return true;
        }
        return false;
    }

    public virtual void TouchPosition()
    {
        position = Input.touches[fingerId].position;
    }
}

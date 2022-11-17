using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.35f;

    public event VoidFunc DoubleClick;

    bool IsDoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicked++;
            if (clicked == 1) clicktime = Time.time;
        }
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            return true;
        }
        else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;
        return false;
    }


    private void Update()
    {

        if (IsDoubleClick())
        {
            DoubleClick?.Invoke();
        }
    }
}

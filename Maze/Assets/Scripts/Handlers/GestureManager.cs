using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public event VoidFunc UpSwipe;
    public event VoidFunc DownSwipe;
    public event VoidFunc RightSwipe;
    public event VoidFunc LeftSwipe;

    float swipeLenght = 0.5f;

    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.35f;

    public event VoidFunc DoubleClick;

    public bool swipeInProgress = false;
    private void Update()
    {
        if (swipeInProgress) return;
        Swipe();
        if (IsDoubleClick())
        {
            DoubleClick?.Invoke();
        }
    }
    public void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            //swipe upwards
            if (currentSwipe.y > 0 && currentSwipe.x > -swipeLenght && currentSwipe.x < swipeLenght)
            {
                Debug.Log("up swipe");
                UpSwipe?.Invoke();
                return;
            }
            //swipe down
            if (currentSwipe.y < 0 && currentSwipe.x > -swipeLenght && currentSwipe.x < swipeLenght)
            {
                Debug.Log("down swipe");
                DownSwipe?.Invoke();
                return;
            }
            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -swipeLenght && currentSwipe.y < swipeLenght)
            {
                Debug.Log("left swipe");
                LeftSwipe?.Invoke();
                return;
            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -swipeLenght && currentSwipe.y < swipeLenght)
            {
                Debug.Log("right swipe");
                RightSwipe?.Invoke();
                return;
            }
        }
    }

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

    public void SetSwipeInProgress(bool value)
    {
        swipeInProgress = value;
    }
}

public enum SwipeDirection {
    Up, Down, Right, Left
}
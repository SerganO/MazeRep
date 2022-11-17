using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISwipeHandler
{
    [HideInInspector]
    public LevelHandler levelHandler;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DownSwipe()
    {
        animator.Play("SwipeDown");
    }

    public void LeftSwipe()
    {
        animator.Play("SwipeLeft");
    }

    public void RightSwipe()
    {
        animator.Play("SwipeRight");
    }

    public void UpSwipe()
    {
        animator.Play("SwipeUp");
    }

    public void WrongSwipe()
    {
        animator.Play("WrongSwipe");
    }

    public void Death()
    {
        animator.Play("Death");
    }

    public void Idle()
    {
        animator.Play("Idle");
    }

    public void Win()
    {
        animator.Play("Win");
    }

    public void DeathAnimationEnded()
    {
        levelHandler.AfterUnitDeathFunction();
    }

    public void WinAnimationEnded()
    {
        levelHandler.AfterWinFunction();
    }

    public void SwipeStarted()
    {
        levelHandler.SwipeStarted();
    }

    public void SwipeFinished()
    {
        levelHandler.SwipeFinished();
    }

}

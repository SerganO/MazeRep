using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemyData data;
    public int currentPointIndex = 0;

    public Vector3 CurrentPos
    {
        get
        {
            return new Vector3((int)transform.position.x, (int)transform.position.y);
        }
    }


    public SwipeDirection NextStepDirection()
    {
        if (data.path.Count == 0) return SwipeDirection.None;
        var x = (int)gameObject.transform.position.x;
        var y = (int)gameObject.transform.position.y;

        if(x == data.path[currentPointIndex].x && y == data.path[currentPointIndex].y)
        {
            if (data.path.Count == 1) return SwipeDirection.None;
            currentPointIndex++;
            if (currentPointIndex == data.path.Count) currentPointIndex = 0;
        }

        var newTile = data.path[currentPointIndex];

        if (newTile.x > x) 
            return SwipeDirection.Right;
        else if (newTile.x < x) 
            return SwipeDirection.Left;
        else if (newTile.y > y) 
            return SwipeDirection.Up;
        else if (newTile.y < y)
            return SwipeDirection.Down;
        else  return SwipeDirection.None;
    } 

}

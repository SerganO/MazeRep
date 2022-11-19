using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public EnemyData data;
    public int currentPointIndex = 0;

    public Position Position;

    public SwipeDirection NextStepDirection()
    {
        if (data.path.Count == 0) return SwipeDirection.None;
        var x = Position.x;
        var y = Position.y;

        if(x == data.path[currentPointIndex].x && y == data.path[currentPointIndex].y)
        {
            if (data.path.Count == 1) return SwipeDirection.None;
            currentPointIndex++;
            if (currentPointIndex >= data.path.Count) currentPointIndex = 0;
        }

        
        var newTile = data.path[currentPointIndex];
        Debug.Log($"{Position} -> {newTile}");
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

    public void SetPosition(Vector3 position)
    {
        Position.x = (int)position.x;
        Position.y = (int)position.y;
    }

    public void SetPosition(Position position)
    {
        Position.x = position.x;
        Position.y = position.y;
    }

    public void Move()
    {
        Move(NextStepDirection());
    }

    public void Move(SwipeDirection direction)
    {
        Debug.Log(direction);
        switch (direction)
        {
            case SwipeDirection.Down:
                DownSwipe();
                Position.y--;
                break;
            case SwipeDirection.Up:
                UpSwipe();
                Position.y++;
                break;
            case SwipeDirection.Right:
                RightSwipe();
                Position.x++;
                break;
            case SwipeDirection.Left:
                LeftSwipe();
                Position.x--;
                break;
            case SwipeDirection.None:
                return;
        }
        MoveToCurrentPosition();

    }


    public void MoveToCurrentPosition()
    {
        transform.parent.position = new Vector3(Position.x, Position.y, 0);
    }

    public void ResetPosition()
    {
        SetPosition(data.startPoint);
        currentPointIndex = 0;
    }

    public void MoveToStartPosition()
    {
        ResetPosition();
        MoveToCurrentPosition();
    }
}

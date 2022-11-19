using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string enemyId;

    public Position startPoint;

    public List<Position> path = new List<Position>();
}

[Serializable] 
public class Position
{
    public int x;
    public int y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return $"{x}:{y}";
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDevObject : MonoBehaviour
{
    public string enemyId;
    public Transform startPoint;
    public List<Transform> pathPoints;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        if (startPoint != null && pathPoints.Count > 0)
        {
            Gizmos.DrawLine(startPoint.position.MovedByXYZ(0.5f, 0.5f), pathPoints[0].position.MovedByXYZ(0.5f, 0.5f));
            for (int i = 1; i < pathPoints.Count; i++)
            {
                Gizmos.DrawLine(pathPoints[i - 1].position.MovedByXYZ(0.5f,0.5f), pathPoints[i].position.MovedByXYZ(0.5f, 0.5f));
            }

            Gizmos.DrawLine(pathPoints[pathPoints.Count - 1].position.MovedByXYZ(0.5f, 0.5f), pathPoints[0].position.MovedByXYZ(0.5f, 0.5f));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (startPoint != null && pathPoints.Count > 0)
        {
            DrawTriangle(startPoint.position.MovedByXYZ(0.5f, 0.5f), pathPoints[0].position.MovedByXYZ(0.5f, 0.5f));
            for (int i = 1; i < pathPoints.Count; i++)
            {
                DrawTriangle(pathPoints[i - 1].position.MovedByXYZ(0.5f, 0.5f), pathPoints[i].position.MovedByXYZ(0.5f, 0.5f));
            }

            DrawTriangle(pathPoints[pathPoints.Count - 1].position.MovedByXYZ(0.5f, 0.5f), pathPoints[0].position.MovedByXYZ(0.5f, 0.5f));
        }
    }

    void DrawTriangle(Vector3 position1, Vector3 position2)
    {
        Gizmos.color = Color.yellow;
        var position = (position1 + position2) / 2;
        var offset = 0.125f;
        if (position1.x == position2.x)
        {
            if(position1.y > position2.y)
            {
                Gizmos.DrawLine(new Vector3(position.x - offset, position.y), new Vector3(position.x + offset, position.y));
                Gizmos.DrawLine(new Vector3(position.x + offset, position.y), new Vector3(position.x, position.y - offset));
                Gizmos.DrawLine(new Vector3(position.x, position.y - offset), new Vector3(position.x - offset, position.y));
            } else
            {
                Gizmos.DrawLine(new Vector3(position.x - offset, position.y), new Vector3(position.x + offset, position.y));
                Gizmos.DrawLine(new Vector3(position.x + offset, position.y), new Vector3(position.x, position.y + offset));
                Gizmos.DrawLine(new Vector3(position.x, position.y + offset), new Vector3(position.x - offset, position.y));
            }
        } else
        {
            if (position1.x > position2.x)
            {
                Gizmos.DrawLine(new Vector3(position.x, position.y + offset), new Vector3(position.x - offset, position.y));
                Gizmos.DrawLine(new Vector3(position.x - offset, position.y), new Vector3(position.x, position.y - offset));
                Gizmos.DrawLine(new Vector3(position.x, position.y - offset), new Vector3(position.x, position.y + offset));
            }
            else
            {
                Gizmos.DrawLine(new Vector3(position.x, position.y + offset), new Vector3(position.x + offset, position.y));
                Gizmos.DrawLine(new Vector3(position.x + offset, position.y), new Vector3(position.x, position.y - offset));
                Gizmos.DrawLine(new Vector3(position.x, position.y - offset), new Vector3(position.x, position.y + offset));
            }
        }
       
        Gizmos.color = Color.red;
    } 
}

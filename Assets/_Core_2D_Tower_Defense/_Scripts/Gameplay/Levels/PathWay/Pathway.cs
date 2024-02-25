using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pathway : MonoBehaviour
{
    public int pathwayID;
    public Vector2 startPoint;
    public List<Vector2> wayPoints;
    public PathwayData pathwayData;

    public void InitPathway(PathwayData data)
    {
        pathwayData = data;
        startPoint = data.startPoint;
        wayPoints = data.waypoints;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPoint, wayPoints[0]);
        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(wayPoints[i], wayPoints[i + 1]);
        }
    }
}

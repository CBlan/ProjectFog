using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPath {

    public readonly Vector3[] lookPoints;
    public readonly int finishLineIndex;

    public AStarPath(Vector3[] waypoints, Vector3 startPos, float turnDist)
    {
        lookPoints = waypoints;
        finishLineIndex = waypoints.Length - 1;


    }
}

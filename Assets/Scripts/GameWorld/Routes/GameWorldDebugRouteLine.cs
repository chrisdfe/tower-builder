using System.Collections;
using System.Collections.Generic;
using TowerBuilder.GameWorld.Map;
using TowerBuilder.Stores.Routes;
using UnityEngine;

public class GameWorldDebugRouteLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    RouteAttempt routeAttempt;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawRouteAttempt(RouteAttempt routeAttempt, int attemptIndex)
    {
        this.routeAttempt = routeAttempt;

        gameObject.name = $"RouteAttempt {attemptIndex}: {routeAttempt.status}";

        lineRenderer.positionCount = routeAttempt.routeSegments.Count;

        int i = 0;
        foreach (RouteSegment segment in routeAttempt.routeSegments)
        {
            Vector3 lineOffset = new Vector3(0, 0, -1 - (float)(attemptIndex) * 0.1f);
            Vector3 linePosition = (
                GameWorldMapCellHelpers.CellCoordinatesToPosition(segment.endNode.cellCoordinates) + lineOffset
            );

            lineRenderer.SetPosition(i, linePosition);
            i++;
        }

        Color lineColor = routeAttempt.status == RouteStatus.Complete ? Color.green : Color.red;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }
}

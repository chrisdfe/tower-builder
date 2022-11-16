using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

public class GameWorldRoutesManager : MonoBehaviour
{
    GameObject debugRouteMarkerPrefab;
    GameObject debugRouteLinePrefab;

    GameWorldDebugRouteMarker debugRouteStartMarker;
    GameWorldDebugRouteMarker debugRouteEndMarker;

    List<GameWorldDebugRouteLine> debugRouteLines = new List<GameWorldDebugRouteLine>();

    void Awake()
    {
        debugRouteMarkerPrefab = Resources.Load<GameObject>("Prefabs/Routes/DebugRouteMarker");
        debugRouteLinePrefab = Resources.Load<GameObject>("Prefabs/Routes/DebugRouteLine");

        Registry.appState.Routes.onDebugRouteStartSet += OnDebugRouteStartSet;
        Registry.appState.Routes.onDebugRouteEndSet += OnDebugRouteEndSet;
        Registry.appState.Routes.onDebugRouteCalculated += OnDebugRouteCalculated;
        Registry.appState.Routes.onDebugRouteCleared += OnDebugRouteCleared;
    }

    void OnDebugRouteStartSet(CellCoordinates cellCoordinates)
    {
        if (debugRouteStartMarker == null)
        {
            debugRouteStartMarker = Instantiate<GameObject>(debugRouteMarkerPrefab).GetComponent<GameWorldDebugRouteMarker>();
            debugRouteStartMarker.transform.parent = transform;
        }

        debugRouteStartMarker.SetCoordinates(cellCoordinates);
    }

    void OnDebugRouteEndSet(CellCoordinates cellCoordinates)
    {
        if (debugRouteEndMarker == null)
        {
            debugRouteEndMarker = Instantiate<GameObject>(debugRouteMarkerPrefab).GetComponent<GameWorldDebugRouteMarker>();
            debugRouteEndMarker.transform.parent = transform;
        }

        debugRouteEndMarker.SetCoordinates(cellCoordinates);
    }

    void OnDebugRouteCleared()
    {
        if (debugRouteStartMarker != null)
        {
            Destroy(debugRouteStartMarker.gameObject);
        }

        if (debugRouteEndMarker != null)
        {
            Destroy(debugRouteEndMarker.gameObject);
        }

        ClearDebugRoutes();
    }

    void ClearDebugRoutes()
    {
        foreach (GameWorldDebugRouteLine line in debugRouteLines)
        {
            Destroy(line.gameObject);
        }

        debugRouteLines = new List<GameWorldDebugRouteLine>();
    }

    void OnDebugRouteCalculated()
    {
        ClearDebugRoutes();
        debugRouteLines = new List<GameWorldDebugRouteLine>();

        List<RouteAttempt> routeAttempts = Registry.appState.Routes.debugRouteAttempts;

        int attemptIndex = 0;
        foreach (RouteAttempt routeAttempt in routeAttempts)
        {
            GameObject debugRouteLineGameObject = Instantiate<GameObject>(debugRouteLinePrefab);
            debugRouteLineGameObject.transform.parent = transform;
            debugRouteLineGameObject.transform.position = Vector3.zero;

            GameWorldDebugRouteLine debugRouteLine = debugRouteLineGameObject.GetComponent<GameWorldDebugRouteLine>();
            debugRouteLine.DrawRouteAttempt(routeAttempt, attemptIndex++);

            debugRouteLines.Add(
                debugRouteLine
            );
        }
    }
}

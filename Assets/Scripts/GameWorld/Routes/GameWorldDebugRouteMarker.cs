using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.GameWorld.Map;
using TowerBuilder.Stores.Map;
using UnityEngine;

public class GameWorldDebugRouteMarker : MonoBehaviour
{
    public void SetCoordinates(CellCoordinates cellCoordinates)
    {
        transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(cellCoordinates);
    }
}

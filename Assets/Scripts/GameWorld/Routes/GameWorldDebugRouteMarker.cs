using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.Stores;

using UnityEngine;

public class GameWorldDebugRouteMarker : MonoBehaviour
{
    public void SetCoordinates(CellCoordinates cellCoordinates)
    {
        transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(cellCoordinates);
    }
}

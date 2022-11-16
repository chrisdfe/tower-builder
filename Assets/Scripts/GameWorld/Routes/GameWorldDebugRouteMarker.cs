using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.GameWorld.Rooms;
using UnityEngine;

public class GameWorldDebugRouteMarker : MonoBehaviour
{
    public void SetCoordinates(CellCoordinates cellCoordinates)
    {
        transform.position = GameWorldMapCellHelpers.CellCoordinatesToPosition(cellCoordinates);
    }
}

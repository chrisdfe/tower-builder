using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.GameWorld;
using TowerBuilder.GameWorld.Rooms;
using UnityEngine;

public class GameWorldDebugRouteMarker : MonoBehaviour
{
    public void SetCoordinates(CellCoordinates cellCoordinates)
    {
        transform.position = GameWorldUtils.CellCoordinatesToPosition(cellCoordinates);
    }
}

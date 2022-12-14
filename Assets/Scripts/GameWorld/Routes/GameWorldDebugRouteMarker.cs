using TowerBuilder.DataTypes;
using TowerBuilder.GameWorld;
using UnityEngine;

public class GameWorldDebugRouteMarker : MonoBehaviour
{
    public void SetCoordinates(CellCoordinates cellCoordinates)
    {
        transform.position = GameWorldUtils.CellCoordinatesToPosition(cellCoordinates);
    }
}

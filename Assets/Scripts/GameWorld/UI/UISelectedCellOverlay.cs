using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Blueprints;
using TowerBuilder.State.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UISelectedCellOverlay : MonoBehaviour
    {
        public void Awake()
        {
            Registry.appState.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates)
        {
            transform.position = new Vector3(
                cellCoordinates.x * Constants.TILE_SIZE,
                cellCoordinates.floor * Constants.TILE_SIZE,
                70
            );
        }

        void SetPosition() { }
    }
}
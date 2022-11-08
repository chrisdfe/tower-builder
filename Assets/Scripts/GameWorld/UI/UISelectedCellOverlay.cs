using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class UISelectedCellOverlay : MonoBehaviour
    {
        const int Z_INDEX = 70;

        public void Awake()
        {
            Registry.appState.UI.events.onSelectionBoxUpdated += OnSelectionBoxUpdated;
        }

        public void OnDestroy()
        {
            Registry.appState.UI.events.onSelectionBoxUpdated -= OnSelectionBoxUpdated;
        }

        void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            ResizeAndSetPosition();
        }

        void ResizeAndSetPosition()
        {
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;
            float width = selectionBox.cellCoordinatesList.GetWidth();
            float height = selectionBox.cellCoordinatesList.GetFloorSpan();
            CellCoordinates bottomLeft = selectionBox.cellCoordinatesList.GetBottomLeftCoordinates();

            transform.position = new Vector3(
                (bottomLeft.x * Constants.TILE_SIZE) + (width / 2) - (Constants.TILE_SIZE / 2),
                (bottomLeft.floor * Constants.TILE_SIZE) + (height / 2) - (Constants.TILE_SIZE / 2),
                Z_INDEX
            );

            transform.localScale = new Vector3(
                width * 0.1f,
                0.1f,
                height * 0.1f
            );
        }
    }
}
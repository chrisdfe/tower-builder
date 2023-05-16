using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.GameWorld;
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
            float width = selectionBox.cellCoordinatesList.width;
            float height = selectionBox.cellCoordinatesList.floorSpan;
            CellCoordinates bottomLeftCoordinates = selectionBox.cellCoordinatesList.bottomLeftCoordinates;

            transform.position = new Vector3(
                (bottomLeftCoordinates.x * Entities.Constants.CELL_WIDTH) + (width / 2) - (Entities.Constants.CELL_WIDTH / 2),
                (bottomLeftCoordinates.floor * Entities.Constants.CELL_HEIGHT) + (height / 2) - (Entities.Constants.CELL_HEIGHT / 2),
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
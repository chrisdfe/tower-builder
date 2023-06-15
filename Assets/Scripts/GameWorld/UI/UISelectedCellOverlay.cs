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
            Registry.appState.UI.onSelectionBoxUpdated += OnSelectionBoxUpdated;
            Registry.appState.UI.onSelectionBoxReset += OnSelectionBoxReset;
        }

        public void OnDestroy()
        {
            Registry.appState.UI.onSelectionBoxUpdated -= OnSelectionBoxUpdated;
            Registry.appState.UI.onSelectionBoxReset -= OnSelectionBoxReset;
        }

        /* 
            Event Handlers
        */
        void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            ResizeAndSetPosition();
        }

        void OnSelectionBoxReset(SelectionBox selectionBox)
        {
            ResizeAndSetPosition();
        }

        /*
            Internals
        */
        void ResizeAndSetPosition()
        {
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;
            float width = selectionBox.cellCoordinatesList.width;
            float height = selectionBox.cellCoordinatesList.height;
            CellCoordinates bottomLeftCoordinates = selectionBox.cellCoordinatesList.bottomLeftCoordinates;

            transform.position = new Vector3(
                (bottomLeftCoordinates.x * Entities.Constants.CELL_WIDTH) + (width / 2) - (Entities.Constants.CELL_WIDTH / 2),
                (bottomLeftCoordinates.y * Entities.Constants.CELL_HEIGHT) + (height / 2) - (Entities.Constants.CELL_HEIGHT / 2),
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
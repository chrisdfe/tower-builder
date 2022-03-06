using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class MapCursor : MonoBehaviour
    {
        public bool isEnabled { get; private set; } = true;

        public RoomBlueprint roomBlueprint { get; private set; }
        public CellCoordinates cellCoordinates { get; private set; }

        GameObject mapCursorCellPrefab;

        List<MapCursorCell> mapCursorCells;

        public void Enable()
        {
            isEnabled = true;
            ResetCursorCells();
        }

        public void Disable()
        {
            isEnabled = false;
            DestroyCursorCells();
        }

        public void ResetCursorCells()
        {
            DestroyCursorCells();

            RoomBlueprint blueprint = Registry.Stores.MapUI.currentBlueprint;

            foreach (RoomBlueprintCell roomBlueprintCell in blueprint.roomBlueprintCells)
            {
                GameObject newCursorCellGameObject = Instantiate<GameObject>(mapCursorCellPrefab);

                MapCursorCell newCursorCell = newCursorCellGameObject.GetComponent<MapCursorCell>();
                newCursorCell.parentMapCursor = this;
                newCursorCell.transform.SetParent(transform);
                newCursorCell.SetRoomBlueprintCell(roomBlueprintCell);
                newCursorCell.UpdateMaterialColor();

                mapCursorCells.Add(newCursorCell);
            }
        }

        void Awake()
        {
            mapCursorCellPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapCursorCell");

            cellCoordinates = CellCoordinates.zero;

            mapCursorCells = new List<MapCursorCell>();

            Registry.Stores.MapUI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.Stores.MapUI.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
            // Registry.Stores.MapUI.onBuildStartCellUpdated += OnBuildStartCellUpdated;
        }

        void SetCurrentCell(CellCoordinates cellCoordinates)
        {
            this.cellCoordinates = cellCoordinates;
            // transform.localPosition = MapCellHelpers.CellCoordinatesToPosition(cellCoordinates) + new Vector3(0, 0, -0.5f);

            ResetCursorCells();
        }

        void DestroyCursorCells()
        {
            if (mapCursorCells.Count > 0)
            {
                foreach (MapCursorCell mapCursorCell in mapCursorCells)
                {
                    Destroy(mapCursorCell.gameObject);
                }
            }

            mapCursorCells = new List<MapCursorCell>();
        }


        void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell)
        {
            SetCurrentCell(currentSelectedCell);
            ResetCursorCells();
        }

        // void OnBuildStartCellUpdated(CellCoordinates buildStartCell)
        // {
        //     ResetCursorCells();
        // }

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            ResetCursorCells();
        }
    }
}

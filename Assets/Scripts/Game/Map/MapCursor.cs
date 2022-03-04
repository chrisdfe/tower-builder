using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class MapCursor : MonoBehaviour
    {
        public bool isVisible { get; private set; } = true;
        public bool isEnabled { get; private set; } = true;

        public RoomBlueprint roomBlueprint { get; private set; }
        public CellCoordinates cellCoordinates { get; private set; }

        GameObject mapCursorCellPrefab;

        List<MapCursorCell> mapCursorCells;

        public void SetCurrentCell(CellCoordinates cellCoordinates)
        {
            this.cellCoordinates = cellCoordinates;
            transform.localPosition = MapCellHelpers.CellCoordinatesToPosition(cellCoordinates) + new Vector3(0, 0, -0.5f);

            ResetCursorCells();
        }

        public void Hide()
        {
            isVisible = false;
            DestroyCursorCells();
        }

        public void Show()
        {
            isVisible = true;
            ResetCursorCells();
        }

        public void Disable()
        {
            isEnabled = false;
            Hide();
        }

        public void Enable()
        {
            isEnabled = true;
            Show();
        }

        public void ResetCursorCells()
        {
            DestroyCursorCells();

            RoomKey currentRoomKey = Registry.Stores.MapUI.selectedRoomKey;
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
            float TILE_SIZE = Stores.Map.Constants.TILE_SIZE;
            transform.localScale = new Vector3(TILE_SIZE, TILE_SIZE, TILE_SIZE);

            mapCursorCellPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapCursorCell");

            cellCoordinates = CellCoordinates.zero;

            mapCursorCells = new List<MapCursorCell>();
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
    }
}

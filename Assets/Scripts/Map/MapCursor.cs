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

        GameObject mapCursorCellPrefab;

        // TODO - this should ultimately live somewhere higher up.
        RoomBlueprint blueprint;
        List<MapCursorCell> mapCursorCells;

        public void SetCurrentTile(CellCoordinates cellCoordinates)
        {
            transform.localPosition = new Vector3(
                MapCellHelpers.RoundToNearestTile(cellCoordinates.x),
                MapCellHelpers.RoundToNearestTile(cellCoordinates.floor),
                -(Stores.Map.Constants.TILE_SIZE)
            );
            Debug.Log(transform.localPosition);
            // (
            //     MapCellHelpers.RoundToNearestTile(Registry.Stores.MapUI.currentFocusFloor) +
            //     (Stores.Map.Constants.TILE_SIZE / 2)
            // ),
        }

        public void Hide()
        {
            isVisible = false;
            // material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
            DestroyCursorCells();
        }

        public void Show()
        {
            isVisible = true;
            // material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
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

        void Awake()
        {
            float TILE_SIZE = Stores.Map.Constants.TILE_SIZE;
            transform.localScale = new Vector3(TILE_SIZE, TILE_SIZE, TILE_SIZE);

            mapCursorCellPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapCursorCell");

            // ResetCursorCells();
            mapCursorCells = new List<MapCursorCell>();

            Registry.Stores.MapUI.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        // Default to a single cursor cell
        void ResetMapCursorCells()
        {
        }

        void ResetCursorCells()
        {
            DestroyCursorCells();

            RoomKey currentRoomKey = Registry.Stores.MapUI.selectedRoomKey;

            blueprint = new RoomBlueprint()
            {
                roomKey = currentRoomKey,
                coordinates = CellCoordinates.zero
            };

            RoomCells blueprintCells = blueprint.GetPositionedRoomCells();

            foreach (CellCoordinates cellCoordinates in blueprintCells.cells)
            {
                GameObject newCursorCell = Instantiate<GameObject>(mapCursorCellPrefab);
                newCursorCell.transform.SetParent(transform);
                newCursorCell.transform.localPosition = MapCellHelpers.CellCoordinatesToPosition(cellCoordinates);
                // TODO - move into a methodn in the MapCursorCell script
                Material cellMaterial = newCursorCell.GetComponent<Renderer>().material;
                cellMaterial.color = new Color(cellMaterial.color.r, cellMaterial.color.g, cellMaterial.color.b, 1);
                mapCursorCells.Add(newCursorCell.GetComponent<MapCursorCell>());
            }
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

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            ResetCursorCells();
        }
    }
}

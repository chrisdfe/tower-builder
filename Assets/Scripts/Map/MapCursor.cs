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

        // Material material;

        GameObject mapCursorCellPrefab;

        // TODO - this should ultimately live somewhere higher up.
        RoomBlueprint blueprint;
        List<MapCursorCell> mapCursorCells;

        public void SetCurrentTile(CellCoordinates cellCoordinates)
        {
            transform.localPosition = new Vector3(
                MapCellHelpers.RoundToNearestTile(cellCoordinates.x),
                (
                    MapCellHelpers.RoundToNearestTile(Registry.Stores.MapUI.currentFocusFloor) +
                    (Stores.Map.Constants.TILE_SIZE / 2)
                ),
                MapCellHelpers.RoundToNearestTile(cellCoordinates.z)
            );
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
            // material = GetComponent<Renderer>().material;
            mapCursorCellPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapCursorCell");

            mapCursorCells = new List<MapCursorCell>();
            // ResetCursorCells();

            Registry.Stores.MapUI.onBlueprintRotationUpdated += OnBlueprintRotationUpdated;
            Registry.Stores.MapUI.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void ResetCursorCells()
        {
            DestroyCursorCells();

            RoomKey currentRoomKey = Registry.Stores.MapUI.selectedRoomKey;
            MapRoomRotation rotation = Registry.Stores.MapUI.currentBlueprintRotation;

            blueprint = new RoomBlueprint()
            {
                roomKey = currentRoomKey,
                rotation = rotation
            };

            RoomCells blueprintCells = blueprint.GetRotatedRoomCells();

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

        void OnBlueprintRotationUpdated(MapRoomRotation rotation)
        {
            ResetCursorCells();
        }

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            ResetCursorCells();
        }
    }
}

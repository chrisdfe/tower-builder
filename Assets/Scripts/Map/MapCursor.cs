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
        MapRoomBlueprint blueprint;
        List<MapCursorCell> mapCursorCells;

        public void SetCurrentTile(CellCoordinates cellCoordinates)
        {
            transform.localPosition = new Vector3(
                MapCellHelpers.RoundToNearestTile(cellCoordinates.x),
                (
                    MapCellHelpers.RoundToNearestTile(Registry.storeRegistry.mapUIStore.state.currentFocusFloor) +
                    (MapStore.Constants.TILE_SIZE / 2)
                ),
                MapCellHelpers.RoundToNearestTile(cellCoordinates.z)
            );
        }

        public void Hide()
        {
            isVisible = false;
            // material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
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
            float TILE_SIZE = MapStore.Constants.TILE_SIZE;
            transform.localScale = new Vector3(TILE_SIZE, TILE_SIZE, TILE_SIZE);
            // material = GetComponent<Renderer>().material;
            mapCursorCellPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapCursorCell");

            mapCursorCells = new List<MapCursorCell>();
            // ResetCursorCells();

            MapUIStore.Events.onBlueprintRotationUpdated += OnBlueprintRotationUpdated;
            MapUIStore.Events.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void ResetCursorCells()
        {
            if (mapCursorCells.Count > 0)
            {
                foreach (MapCursorCell mapCursorCell in mapCursorCells)
                {
                    Destroy(mapCursorCell.gameObject);
                }
            }

            mapCursorCells = new List<MapCursorCell>();

            RoomKey currentRoomKey = Registry.storeRegistry.mapUIStore.state.selectedRoomKey;
            MapRoomRotation rotation = Registry.storeRegistry.mapUIStore.state.currentBlueprintRotation;

            blueprint = new MapRoomBlueprint()
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

        void OnBlueprintRotationUpdated(MapRoomRotation rotation, MapRoomRotation previousRotation)
        {
            // switch (rotation)
            // {
            //     case MapRoomRotation.Right:
            //         transform.localRotation = Quaternion.identity;
            //         break;
            //     case MapRoomRotation.Down:
            //         transform.localRotation = Quaternion.Euler(90, 0, 0);
            //         break;
            //     case MapRoomRotation.Left:
            //         transform.localRotation = Quaternion.Euler(180, 0, 0);
            //         break;
            //     case MapRoomRotation.Up:
            //         transform.localRotation = Quaternion.Euler(-90, 0, 0);
            //         break;
            //     default:
            //         break;
            // }
            ResetCursorCells();
        }

        void OnSelectedRoomKeyUpdated(MapUIStore.Events.StateEventPayload payload)
        {
            ResetCursorCells();
        }
    }
}

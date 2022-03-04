using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class MapManager : MonoBehaviour
    {
        public Transform buildingWrapper;

        public FloorPlane floorPlane;
        public Collider floorPlaneCollider;

        GameObject mapCursorPrefab;
        public MapCursor mapCursor;

        GameObject placeholderTileCubePrefab;
        ToolStateHandlersBase currentToolStateHandler;

        public Dictionary<ToolState, ToolStateHandlersBase> toolStateHandlerMap;

        // Distance from the edge of the screen where the mapCursor will get disabled
        // TODO - this should perhaps be percentages instead
        // TODO - move to MapUI.Constants
        public static Vector2 MAP_CURSOR_CLICK_BUFFER = new Vector2(150, 150);

        void Awake()
        {
            floorPlane = transform.Find("FloorPlane").GetComponent<FloorPlane>();
            floorPlaneCollider = floorPlane.GetComponent<Collider>();

            mapCursorPrefab = Resources.Load<GameObject>("Prefabs/MapUI/MapCursor");
            mapCursor = Instantiate<GameObject>(mapCursorPrefab).GetComponent<MapCursor>();
            mapCursor.transform.SetParent(transform);
            mapCursor.transform.position = Vector3.zero;
            mapCursor.Disable();

            placeholderTileCubePrefab = Resources.Load<GameObject>("Prefabs/Map/PlaceholderTileCube");

            buildingWrapper = transform.Find("BuildingWrapper");

            toolStateHandlerMap = new Dictionary<ToolState, ToolStateHandlersBase>()
            {
                [ToolState.None] = new NoneToolStateHandlers(this),
                [ToolState.Build] = new BuildToolStateHandlers(this),
                [ToolState.Inspect] = new InspectToolStateHandlers(this),
                [ToolState.Destroy] = new DestroyToolStateHandlers(this),
            };

            SetCurrentToolStateHandlers();

            // Perform initialization of whatever tool state is the default
            currentToolStateHandler.OnTransitionTo(Registry.Stores.MapUI.toolState);

            Registry.Stores.MapUI.onToolStateUpdated += OnToolStateUpdated;
            Registry.Stores.MapUI.onCurrentSelectedTileUpdated += OnCurrentSelectedTileUpdated;
            Registry.Stores.MapUI.onSelectedRoomKeyUpdated += OnSelectedRoomKeyUpdated;
        }

        void Update()
        {
            // TODO - handle transitions between "is in dead zone" and "is not in dead zone"
            if (!MouseCursorIsInDeadZone())
            {
                UpdateMapCursor();

                if (Input.GetMouseButtonDown(0))
                {
                    currentToolStateHandler.OnMouseDown();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    currentToolStateHandler.OnMouseUp();
                }
            }

            currentToolStateHandler.Update();
        }

        bool MouseCursorIsInDeadZone()
        {
            return (
                Input.mousePosition.x < MAP_CURSOR_CLICK_BUFFER.x ||
                Input.mousePosition.x > (Screen.width - MAP_CURSOR_CLICK_BUFFER.x) ||

                Input.mousePosition.y < MAP_CURSOR_CLICK_BUFFER.y ||
                Input.mousePosition.y > (Screen.height - MAP_CURSOR_CLICK_BUFFER.y)
            );
        }

        void UpdateMapCursor()
        {
            if (!mapCursor.isEnabled)
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (floorPlaneCollider.Raycast(ray, out hit, 100))
            {
                CellCoordinates hoveredCell = new CellCoordinates()
                {
                    x = MapCellHelpers.RoundToNearestTile(hit.point.x),
                    floor = MapCellHelpers.RoundToNearestTile(hit.point.y),
                };

                if (!hoveredCell.Matches(Registry.Stores.MapUI.currentSelectedTile))
                {
                    Registry.Stores.MapUI.SetCurrentSelectedCell(hoveredCell);
                }
            }
        }

        void OnToolStateUpdated(ToolState nextToolState, ToolState previousToolState)
        {
            currentToolStateHandler.OnTransitionFrom(nextToolState);
            SetCurrentToolStateHandlers();
            currentToolStateHandler.OnTransitionTo(previousToolState);
        }

        void OnCurrentSelectedTileUpdated(CellCoordinates currentSelectedTile)
        {
            mapCursor.SetCurrentCell(currentSelectedTile);
            mapCursor.ResetCursorCells();
        }

        void OnSelectedRoomKeyUpdated(RoomKey selectedRoomKey)
        {
            mapCursor.ResetCursorCells();
        }

        void SetCurrentToolStateHandlers()
        {
            ToolState currentToolState = Registry.Stores.MapUI.toolState;
            currentToolStateHandler = toolStateHandlerMap[currentToolState];
        }
    }
}

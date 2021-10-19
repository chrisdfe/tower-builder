using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.UI
{
    public class MapManager : MonoBehaviour
    {
        public Transform buildingWrapper;

        // public GameObject floorPlanePrefab;
        public FloorPlane floorPlane;
        public Collider floorPlaneCollider;

        GameObject mapCursorPrefab;
        public MapCursor mapCursor;

        GameObject placeholderTileCubePrefab;

        Vector2 currentHoveredTile;
        ToolStateHandlersBase currentToolStateHandler;

        public Dictionary<ToolState, ToolStateHandlersBase> toolStateHandlerMap;

        // Distance from the edge of the screen where the mapCursor will get disabled
        // TODO - this should perhaps be percentages instead
        // TODO - move to MapUIConstants
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
            currentToolStateHandler.OnTransitionTo(Registry.Stores.mapUIStore.state.toolState);

            MapUIStore.Events.onToolStateUpdated += OnToolStateUpdated;
            MapUIStore.Events.onCurrentSelectedTileUpdated += OnCurrentSelectedTileUpdated;
            MapStore.Events.onMapRoomAdded += OnMapRoomAdded;
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

            // TODO - move somewhere more specific to floor stuff?
            if (Input.GetKeyDown("]"))
            {
                FocusFloorUp();
            }

            if (Input.GetKeyDown("["))
            {
                FocusFloorDown();
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
                    z = MapCellHelpers.RoundToNearestTile(hit.point.z),
                    floor = Registry.Stores.mapUIStore.state.currentFocusFloor
                };

                CellCoordinates currentSelectedTile = Registry.Stores.mapUIStore.state.currentSelectedTile;

                if (!currentSelectedTile.Matches(hoveredCell))
                {
                    MapUIStore.Mutations.SetCurrentSelectedCell(hoveredCell);
                }
            }
        }

        void FocusFloorUp()
        {
            int currentFocusFloor = Registry.Stores.mapUIStore.state.currentFocusFloor;
            // TODO - cap at highest floor
            int newFocusFloor = currentFocusFloor + 1;
            Stores.MapUI.MapUIStore.Mutations.SetCurrentFocusFloor(newFocusFloor);
        }

        void FocusFloorDown()
        {
            int currentFocusFloor = Registry.Stores.mapUIStore.state.currentFocusFloor;
            // TODO - cap at lowest floor
            int newFocusFloor = currentFocusFloor - 1;
            Stores.MapUI.MapUIStore.Mutations.SetCurrentFocusFloor(newFocusFloor);
        }

        void OnToolStateUpdated(MapUIStore.Events.StateEventPayload payload)
        {
            ToolState previousToolState = payload.previousState.toolState;
            ToolState nextToolState = payload.state.toolState;

            currentToolStateHandler.OnTransitionFrom(nextToolState);
            SetCurrentToolStateHandlers();
            currentToolStateHandler.OnTransitionTo(previousToolState);
        }

        void OnCurrentSelectedTileUpdated(MapUIStore.Events.StateEventPayload payload)
        {
            CellCoordinates currentSelectedTile = payload.state.currentSelectedTile;
            mapCursor.SetCurrentTile(currentSelectedTile);
        }

        void OnMapRoomAdded(MapRoom newMapRoom)
        {
            Debug.Log("OnMapRoomsAdded");
            float TILE_SIZE = MapStore.Constants.TILE_SIZE;

            foreach (CellCoordinates roomCell in newMapRoom.roomCells.cells)
            {
                GameObject placeholderTile = Instantiate<GameObject>(placeholderTileCubePrefab);
                placeholderTile.transform.position = new Vector3(
                    roomCell.x * TILE_SIZE,
                    roomCell.floor * TILE_SIZE + (TILE_SIZE / 2),
                    roomCell.z * TILE_SIZE
                );
            }
        }

        void SetCurrentToolStateHandlers()
        {
            ToolState currentToolState = Registry.Stores.mapUIStore.state.toolState;
            Debug.Log(currentToolState);
            currentToolStateHandler = toolStateHandlerMap[currentToolState];
        }
    }
}

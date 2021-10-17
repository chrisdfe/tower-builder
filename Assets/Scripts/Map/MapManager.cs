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

        Vector2 currentHoveredTile;
        ToolStateHandlersBase currentToolStateHandler;

        public Dictionary<ToolState, ToolStateHandlersBase> toolStateHandlerMap;

        // Distance from the edge of the screen where the mapCursor will get disabled
        // TODO - this should perhaps be percentages instead
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
            currentToolStateHandler.OnTransitionTo(Registry.storeRegistry.mapUIStore.state.toolState);

            MapUIStore.StateChangeSelectors.onToolStateUpdated += OnToolStateUpdated;
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
                mapCursor.SetCurrentTile(new CellCoordinates()
                {
                    x = MapCellHelpers.RoundToNearestTile(hit.point.x),
                    z = MapCellHelpers.RoundToNearestTile(hit.point.z),
                    floor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor
                });
            }
        }

        void FocusFloorUp()
        {
            int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;
            // TODO - cap at highest floor
            int newFocusFloor = currentFocusFloor + 1;
            Stores.MapUI.MapUIStore.Mutations.SetCurrentFocusFloor(newFocusFloor);
        }

        void FocusFloorDown()
        {
            int currentFocusFloor = Registry.storeRegistry.mapUIStore.state.currentFocusFloor;
            // TODO - cap at lowest floor
            int newFocusFloor = currentFocusFloor - 1;
            Stores.MapUI.MapUIStore.Mutations.SetCurrentFocusFloor(newFocusFloor);
        }

        void OnToolStateUpdated(MapUIStore.StateEventPayload payload)
        {
            ToolState previousToolState = payload.previousState.toolState;
            ToolState nextToolState = payload.state.toolState;

            currentToolStateHandler.OnTransitionFrom(nextToolState);
            SetCurrentToolStateHandlers();
            currentToolStateHandler.OnTransitionTo(previousToolState);
        }

        void OnRoomAdded(MapUIStore.StateEventPayload payload) { }

        void SetCurrentToolStateHandlers()
        {
            ToolState currentToolState = Registry.storeRegistry.mapUIStore.state.toolState;
            Debug.Log(currentToolState);
            currentToolStateHandler = toolStateHandlerMap[currentToolState];
        }
    }
}

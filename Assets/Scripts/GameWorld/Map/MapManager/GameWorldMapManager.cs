using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class GameWorldMapManager : MonoBehaviour
    {
        public Transform roomList;

        public GameWorldFloorPlane floorPlane;
        public Collider floorPlaneCollider;

        ToolStateInputHandlersBase currentToolStateHandler;

        public Dictionary<ToolState, ToolStateInputHandlersBase> toolStateHandlerMap;

        // Distance from the edge of the screen where the mapCursor will get disabled
        // TODO - this should perhaps be percentages instead
        // TODO - move to MapUI.Constants
        public static Vector2 MAP_CURSOR_CLICK_BUFFER = new Vector2(150, 250);

        void Awake()
        {
            floorPlane = transform.Find("FloorPlane").GetComponent<GameWorldFloorPlane>();
            floorPlaneCollider = floorPlane.GetComponent<Collider>();

            InitializeToolStateHandlers();

            roomList = transform.Find("RoomList");
        }

        void Update()
        {
            UpdateCurrentSelectedCell();

            // TODO - handle transitions between "is in dead zone" and "is not in dead zone"
            if (!MouseCursorIsInDeadZone())
            {
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

        // TODO - this should go in GameWorldFloorPlane
        void UpdateCurrentSelectedCell()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (floorPlaneCollider.Raycast(ray, out hit, 100))
            {
                CellCoordinates hoveredCell = new CellCoordinates(
                    GameWorldMapCellHelpers.RoundToNearestTile(hit.point.x),
                    GameWorldMapCellHelpers.RoundToNearestTile(hit.point.y)
                );

                if (!hoveredCell.Matches(Registry.Stores.MapUI.currentSelectedCell))
                {
                    Registry.Stores.MapUI.SetCurrentSelectedCell(hoveredCell);
                }
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

        void InitializeToolStateHandlers()
        {
            toolStateHandlerMap = new Dictionary<ToolState, ToolStateInputHandlersBase>()
            {
                [ToolState.None] = new NoneToolStateInputHandlers(this),
                [ToolState.Build] = new BuildToolStateInputHandlers(this),
                [ToolState.Inspect] = new InspectToolStateInputHandlers(this),
                [ToolState.Destroy] = new DestroyToolStateInputHandlers(this),
                [ToolState.Routes] = new RoutesToolStateInputHandlers(this),
            };

            SetCurrentToolStateHandlers();
            // Perform initialization of whatever tool state is the default
            currentToolStateHandler.OnTransitionTo(Registry.Stores.MapUI.toolState);

            Registry.Stores.MapUI.onToolStateUpdated += OnToolStateUpdated;
        }

        void OnToolStateUpdated(ToolState nextToolState, ToolState previousToolState)
        {
            currentToolStateHandler.OnTransitionFrom(nextToolState);
            SetCurrentToolStateHandlers();
            currentToolStateHandler.OnTransitionTo(previousToolState);
        }

        void SetCurrentToolStateHandlers()
        {
            ToolState currentToolState = Registry.Stores.MapUI.toolState;
            currentToolStateHandler = toolStateHandlerMap[currentToolState];
        }
    }
}

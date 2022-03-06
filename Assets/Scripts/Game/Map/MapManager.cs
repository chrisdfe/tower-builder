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

        public FloorPlane floorPlane;
        public Collider floorPlaneCollider;

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

        void UpdateCurrentSelectedCell()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (floorPlaneCollider.Raycast(ray, out hit, 100))
            {
                CellCoordinates hoveredCell = new CellCoordinates()
                {
                    x = MapCellHelpers.RoundToNearestTile(hit.point.x),
                    floor = MapCellHelpers.RoundToNearestTile(hit.point.y),
                };

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
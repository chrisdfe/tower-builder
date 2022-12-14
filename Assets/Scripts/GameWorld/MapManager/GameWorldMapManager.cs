using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class GameWorldMapManager : MonoBehaviour
    {
        public GameWorldFloorPlane floorPlane;

        [HideInInspector]
        public Collider floorPlaneCollider;

        ToolStateInputHandlersBase currentToolStateHandler;

        public Dictionary<ToolState, ToolStateInputHandlersBase> toolStateHandlerMap;

        // Distance from the edge of the screen where the mapCursor will get disabled
        // TODO - this should perhaps be percentages instead
        // TODO - move to UI.Constants
        public static Vector2 MAP_CURSOR_CLICK_BUFFER = new Vector2(150, 250);

        int selectableEntityLayerMask;
        int uiLayer;
        UIManager uiManager;

        /* 
            Lifecycle Methods
        */
        void Awake()
        {
            floorPlane = transform.Find("FloorPlane").GetComponent<GameWorldFloorPlane>();
            floorPlaneCollider = floorPlane.GetComponent<Collider>();

            SetupToolStateHandlers();

            // make a bit mask
            selectableEntityLayerMask = 1 << LayerMask.NameToLayer("Selectable Entities");
            uiLayer = LayerMask.NameToLayer("UI");
            uiManager = UIManager.Find();
        }

        void Update()
        {
            // UpdateSelectableEntityStack();
            UpdateCurrentSelectedCell();

            if (Input.GetButtonDown("Alt Action"))
            {
                Registry.appState.UI.AltActionStart();
            }
            else if (Input.GetButtonUp("Alt Action"))
            {
                Registry.appState.UI.AltActionEnd();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!uiManager.mouseIsOverUI)
                {
                    // Registry.appState.UI.SelectStart();
                    Registry.appState.UI.LeftClickStart();
                    // currentToolStateHandler.OnMouseDown();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // if (!uiManager.mouseIsOverUI)
                // {
                Registry.appState.UI.LeftClickEnd();
                // currentToolStateHandler.OnMouseUp();
                // }
            }

            // Right click to exit out of current state?
            if (Input.GetMouseButtonDown(1) && Registry.appState.Tools.toolState != ApplicationState.Tools.State.DEFAULT_TOOL_STATE)
            {
                Registry.appState.Tools.SetToolState(ApplicationState.Tools.State.DEFAULT_TOOL_STATE);
            }

            currentToolStateHandler.Update();
        }

        /* 
            Internals
        */
        void UpdateCurrentSelectedCell()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (floorPlaneCollider.Raycast(ray, out hit, 1000))
            {
                CellCoordinates hoveredCell = GameWorldUtils.RoundVector2ToCellCoordinates(
                    new Vector2(hit.point.x, hit.point.y)
                );

                if (!hoveredCell.Matches(Registry.appState.UI.currentSelectedCell))
                {
                    Registry.appState.UI.SetCurrentSelectedCell(hoveredCell);
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

        void SetupToolStateHandlers()
        {
            toolStateHandlerMap = new Dictionary<ToolState, ToolStateInputHandlersBase>()
            {
                [ToolState.Inspect] = new InspectToolStateInputHandlers(this),
                [ToolState.Build] = new BuildToolStateInputHandlers(this),
                [ToolState.Destroy] = new DestroyToolStateInputHandlers(this),
            };

            SetCurrentToolStateHandlers();
            // Perform initialization of whatever tool state is the default
            currentToolStateHandler.OnTransitionTo(Registry.appState.Tools.toolState);

            Registry.appState.Tools.events.onToolStateUpdated += OnToolStateUpdated;
        }

        void OnToolStateUpdated(ToolState nextToolState, ToolState previousToolState)
        {
            currentToolStateHandler.OnTransitionFrom(nextToolState);
            SetCurrentToolStateHandlers();
            currentToolStateHandler.OnTransitionTo(previousToolState);
        }

        void SetCurrentToolStateHandlers()
        {
            ToolState currentToolState = Registry.appState.Tools.toolState;
            currentToolStateHandler = toolStateHandlerMap[currentToolState];
        }
    }
}

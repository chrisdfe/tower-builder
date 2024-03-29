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

        public Dictionary<State.Key, ToolStateInputHandlersBase> toolStateHandlerMap;

        // Distance from the edge of the screen where the mapCursor will get disabled
        // TODO - this should perhaps be percentages instead
        // TODO - move to UI.Constants
        public static Vector2 MAP_CURSOR_CLICK_BUFFER = new Vector2(150, 250);

        int selectableEntityLayerMask;
        int uiLayer;

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
        }

        void Update()
        {
            UpdateCurrentSelectedCell();

            currentToolStateHandler.Update();
        }

        /* 
            Internals
        */
        void UpdateCurrentSelectedCell()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (floorPlaneCollider.Raycast(ray, out hit, 1000))
            {
                CellCoordinates hoveredCell = GameWorldUtils.RoundVector2ToCellCoordinates(
                    new Vector2(hit.point.x, hit.point.y)
                );

                if (!hoveredCell.Matches(Registry.appState.UI.selectedCell))
                {
                    Registry.appState.UI.SetCurrentSelectedCell(hoveredCell);
                }
            }
        }

        /*
        bool MouseCursorIsInDeadZone()
        {
            return (
                Input.mousePosition.x < MAP_CURSOR_CLICK_BUFFER.x ||
                Input.mousePosition.x > (Screen.width - MAP_CURSOR_CLICK_BUFFER.x) ||

                Input.mousePosition.y < MAP_CURSOR_CLICK_BUFFER.y ||
                Input.mousePosition.y > (Screen.height - MAP_CURSOR_CLICK_BUFFER.y)
            );
        }
        */

        void SetupToolStateHandlers()
        {
            toolStateHandlerMap = new Dictionary<ApplicationState.Tools.State.Key, ToolStateInputHandlersBase>()
            {
                [ApplicationState.Tools.State.Key.Inspect] = new InspectToolStateInputHandlers(this),
                [ApplicationState.Tools.State.Key.Build] = new BuildToolStateInputHandlers(this),
                [ApplicationState.Tools.State.Key.Destroy] = new DestroyToolStateInputHandlers(this),
            };

            SetCurrentToolStateHandlers();
            // Perform initialization of whatever tool state is the default
            currentToolStateHandler.OnTransitionTo(Registry.appState.Tools.currentKey);

            Registry.appState.Tools.onToolStateUpdated += OnToolStateUpdated;
        }

        void OnToolStateUpdated(ApplicationState.Tools.State.Key nextToolState, ApplicationState.Tools.State.Key previousToolState)
        {
            currentToolStateHandler.OnTransitionFrom(nextToolState);
            SetCurrentToolStateHandlers();
            currentToolStateHandler.OnTransitionTo(previousToolState);
        }

        void SetCurrentToolStateHandlers()
        {
            ApplicationState.Tools.State.Key currentToolState = Registry.appState.Tools.currentKey;
            currentToolStateHandler = toolStateHandlerMap[currentToolState];
        }
    }
}

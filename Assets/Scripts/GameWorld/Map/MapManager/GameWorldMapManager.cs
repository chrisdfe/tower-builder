using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.GameWorld.Rooms;
using TowerBuilder.GameWorld.UI;
using TowerBuilder.State;
using TowerBuilder.State.Tools;
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
        // TODO - move to UI.Constants
        public static Vector2 MAP_CURSOR_CLICK_BUFFER = new Vector2(150, 250);

        int selectableEntityLayerMask;
        int uiLayer;
        UIManager uiManager;

        void Awake()
        {
            floorPlane = transform.Find("FloorPlane").GetComponent<GameWorldFloorPlane>();
            floorPlaneCollider = floorPlane.GetComponent<Collider>();

            InitializeToolStateHandlers();

            roomList = transform.Find("RoomList");

            // make a bit mask
            selectableEntityLayerMask = 1 << LayerMask.NameToLayer("Selectable Entities");
            uiLayer = LayerMask.NameToLayer("UI");
            uiManager = UIManager.Find();
        }

        void Update()
        {
            UpdateSelectableEntityStack();
            UpdateCurrentSelectedCell();

            if (Input.GetMouseButtonDown(0))
            {
                if (!uiManager.mouseIsOverUI)
                {
                    currentToolStateHandler.OnMouseDown();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!uiManager.mouseIsOverUI)
                {
                    currentToolStateHandler.OnMouseUp();
                }
            }

            currentToolStateHandler.Update();
        }

        // TODO - this probably belongs in UIManager
        void UpdateSelectableEntityStack()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray, 1000, selectableEntityLayerMask);

            SelectableEntityStack stack = new SelectableEntityStack();

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit otherHit = hits[i];

                    // TODO - this should be called GameWorldSelectableEntity
                    SelectableEntity selectableEntity = otherHit.transform.GetComponent<SelectableEntity>();

                    // TODO - I think this is the wrong way around
                    if (selectableEntity)
                    {
                        EntityBase entity = selectableEntity.entity;
                        stack.Push(entity);
                    }
                }
            }

            // TODO - perhaps avoid doing this ever frame
            // Registry.appState.UI.SetEntityStack(stack);
        }

        void UpdateCurrentSelectedCell()
        {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (floorPlaneCollider.Raycast(ray, out hit, 1000))
            {
                CellCoordinates hoveredCell = new CellCoordinates(
                    GameWorldMapCellHelpers.RoundToNearestTile(hit.point.x),
                    GameWorldMapCellHelpers.RoundToNearestTile(hit.point.y)
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

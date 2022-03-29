using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class State
    {
        public ToolState toolState { get; private set; }

        public delegate void ToolStateEvent(ToolState toolState, ToolState previousToolState);
        public ToolStateEvent onToolStateUpdated;

        public CellCoordinates currentSelectedCell { get; private set; } = null;
        public delegate void cellCoordinatesEvent(CellCoordinates currentSelectedCell);
        public cellCoordinatesEvent onCurrentSelectedCellUpdated;

        public RoomConnections blueprintRoomConnections = new RoomConnections();

        public Room currentSelectedRoom { get; private set; } = null;
        public delegate void selectedRoomEvent(Room room);
        public selectedRoomEvent onCurrentSelectedRoomUpdated;

        public NoneToolState noneToolSubState;
        public BuildToolState buildToolSubState;
        public DestroyToolState destroyToolSubState;
        public InspectToolState inspectToolSubState;
        public RoutesToolState routesToolSubState;

        public State()
        {
            toolState = ToolState.None;
            currentSelectedCell = CellCoordinates.zero;

            noneToolSubState = new NoneToolState(this);
            buildToolSubState = new BuildToolState(this);
            destroyToolSubState = new DestroyToolState(this);
            inspectToolSubState = new InspectToolState(this);
            routesToolSubState = new RoutesToolState(this);
        }

        public void SetToolState(ToolState toolState)
        {
            GetCurrentActiveToolSubState().Teardown();

            ToolState previousToolState = this.toolState;
            this.toolState = toolState;

            GetCurrentActiveToolSubState().Setup();

            if (onToolStateUpdated != null)
            {
                onToolStateUpdated(toolState, previousToolState);
            }
        }

        public void SetCurrentSelectedCell(CellCoordinates currentSelectedCell)
        {
            this.currentSelectedCell = currentSelectedCell;

            Room currentRoom = Registry.Stores.Map.rooms.FindRoomAtCell(currentSelectedCell);
            this.currentSelectedRoom = currentRoom;

            GetCurrentActiveToolSubState().OnCurrentSelectedCellUpdated(currentSelectedCell);

            if (onCurrentSelectedCellUpdated != null)
            {
                onCurrentSelectedCellUpdated(currentSelectedCell);
            }

            if (onCurrentSelectedRoomUpdated != null)
            {
                onCurrentSelectedRoomUpdated(this.currentSelectedRoom);
            }
        }

        ToolStateBase GetCurrentActiveToolSubState()
        {

            if (toolState == ToolState.Build)
            {
                return buildToolSubState;
            }

            if (toolState == ToolState.Destroy)
            {
                return destroyToolSubState;
            }

            if (toolState == ToolState.Inspect)
            {
                return inspectToolSubState;
            }

            if (toolState == ToolState.Routes)
            {
                return routesToolSubState;
            }

            return noneToolSubState;
        }
    }
}


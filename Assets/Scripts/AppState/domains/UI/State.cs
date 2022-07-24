using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.State.UI
{
    public class State
    {
        public struct Input
        {
            public ToolState? toolState;
            public CellCoordinates currentSelectedCell;

            public NoneToolState.Input noneToolState;
            public BuildToolState.Input buildToolState;
            public DestroyToolState.Input destroyToolState;
            public InspectToolState.Input inspectToolState;
            public RoutesToolState.Input routesToolState;
        }

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

        public RoomCells currentSelectedRoomBlock { get; private set; } = null;
        public delegate void selectedRoomBlockEvent(RoomCells roomBlock);
        public selectedRoomBlockEvent onCurrentSelectedRoomBlockUpdated;

        public NoneToolState noneToolSubState;
        public BuildToolState buildToolSubState;
        public DestroyToolState destroyToolSubState;
        public InspectToolState inspectToolSubState;
        public RoutesToolState routesToolSubState;

        public SelectableEntityStack selectableEntityStack { get; private set; } = new SelectableEntityStack();

        public State() : this(new Input()) { }

        public State(Input input)
        {
            toolState = input.toolState ?? ToolState.None;
            currentSelectedCell = input.currentSelectedCell ?? CellCoordinates.zero;

            noneToolSubState = new NoneToolState(this, input.noneToolState);
            buildToolSubState = new BuildToolState(this, input.buildToolState);
            destroyToolSubState = new DestroyToolState(this, input.destroyToolState);
            inspectToolSubState = new InspectToolState(this, input.inspectToolState);
            routesToolSubState = new RoutesToolState(this, input.routesToolState);
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

            currentSelectedRoom = Registry.appState.Rooms.FindRoomAtCell(currentSelectedCell);

            currentSelectedRoomBlock = null;
            if (currentSelectedRoom != null)
            {
                currentSelectedRoomBlock = currentSelectedRoom.FindBlockByCellCoordinates(currentSelectedCell);
            }

            ToolStateBase currentToolState = GetCurrentActiveToolSubState();
            currentToolState.OnCurrentSelectedCellUpdated(currentSelectedCell);
            currentToolState.OnCurrentSelectedRoomUpdated(currentSelectedRoom);
            currentToolState.OnCurrentSelectedRoomBlockUpdated(currentSelectedRoomBlock);

            if (onCurrentSelectedCellUpdated != null)
            {
                onCurrentSelectedCellUpdated(currentSelectedCell);
            }


            if (onCurrentSelectedRoomUpdated != null)
            {
                onCurrentSelectedRoomUpdated(currentSelectedRoom);
            }

            if (onCurrentSelectedRoomBlockUpdated != null)
            {
                onCurrentSelectedRoomBlockUpdated(currentSelectedRoomBlock);
            }
        }

        public void SetEntityStack(SelectableEntityStack stack)
        {
            this.selectableEntityStack = stack;
            // Debug.Log(stack.Count);
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


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

        public ResourceStructField<ToolState> toolState { get; private set; } = new ResourceStructField<ToolState>(ToolState.None);

        public CellCoordinates currentSelectedCell { get; private set; } = null;
        public delegate void cellCoordinatesEvent(CellCoordinates currentSelectedCell);
        public cellCoordinatesEvent onCurrentSelectedCellUpdated;

        public RoomConnections blueprintRoomConnections = new RoomConnections();

        public Room currentSelectedRoom { get; private set; } = null;
        public delegate void selectedRoomEvent(Room room);
        public selectedRoomEvent onCurrentSelectedRoomUpdated;

        public RoomCells currentSelectedRoomBlock { get; private set; } = null;
        public delegate void SelectedRoomBlockEvent(RoomCells roomBlock);
        public SelectedRoomBlockEvent onCurrentSelectedRoomBlockUpdated;

        public NoneToolState noneToolSubState;
        public BuildToolState buildToolSubState;
        public DestroyToolState destroyToolSubState;
        public InspectToolState inspectToolSubState;
        public RoutesToolState routesToolSubState;

        public SelectableEntityStack selectableEntityStack { get; private set; } = new SelectableEntityStack();

        public State(Input input)
        {
            toolState.onValueChanged += TransitionToolState;
            toolState.value = input.toolState ?? ToolState.None;

            currentSelectedCell = input.currentSelectedCell ?? CellCoordinates.zero;

            noneToolSubState = new NoneToolState(this, input.noneToolState);
            buildToolSubState = new BuildToolState(this, input.buildToolState);
            destroyToolSubState = new DestroyToolState(this, input.destroyToolState);
            inspectToolSubState = new InspectToolState(this, input.inspectToolState);
            routesToolSubState = new RoutesToolState(this, input.routesToolState);

        }

        public State() : this(new Input()) { }

        public void TransitionToolState(ToolState toolState, ToolState previousToolState)
        {
            Debug.Log("transitioning from " + previousToolState);
            Debug.Log("to " + toolState);
            GetToolSubState(previousToolState).Teardown();
            GetToolSubState(toolState).Setup();
        }

        // public void SetToolState(ToolState toolState)
        // {
        //     GetCurrentActiveToolSubState().Teardown();

        //     ToolState previousToolState = this.toolState.value;
        // }

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
            return GetToolSubState(toolState.value);
        }

        ToolStateBase GetToolSubState(ToolState toolState)
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


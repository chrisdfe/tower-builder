using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public class State : StateSlice
    {
        public static ToolState DEFAULT_TOOL_STATE = ToolState.Inspect;

        public struct Input
        {
            public ToolState? toolState;
            public CellCoordinates currentSelectedCell;

            // public NoneToolState.Input noneToolState;
            public BuildToolState.Input buildToolState;
            public DestroyToolState.Input destroyToolState;
            public InspectToolState.Input inspectToolState;
            public RoutesToolState.Input routesToolState;
        }

        public class Events
        {
            public delegate void ToolStateUpdatedEvent(ToolState toolState, ToolState previousToolState);
            public ToolStateUpdatedEvent onToolStateUpdated;
        }

        public ToolState toolState { get; private set; } = DEFAULT_TOOL_STATE;

        // public NoneToolState noneToolState;
        public BuildToolState buildToolState;
        public DestroyToolState destroyToolState;
        public InspectToolState inspectToolState;
        public RoutesToolState routesToolState;

        public Events events;

        ToolStateBase activeToolState { get { return GetToolState(toolState); } }

        public State(AppState appState, Input input) : base(appState)
        {
            toolState = input.toolState ?? DEFAULT_TOOL_STATE;

            // noneToolState = new NoneToolState(this, input.noneToolState);
            buildToolState = new BuildToolState(appState, this, input.buildToolState);
            destroyToolState = new DestroyToolState(appState, this, input.destroyToolState);
            inspectToolState = new InspectToolState(appState, this, input.inspectToolState);
            routesToolState = new RoutesToolState(appState, this, input.routesToolState);

            events = new State.Events();

            appState.UI.events.onSelectionStart += OnSelectionStart;
            appState.UI.events.onSelectionEnd += OnSelectionEnd;
            appState.UI.events.onSelectionBoxUpdated += OnSelectionBoxUpdated;

            appState.UI.events.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            appState.UI.events.onCurrentSelectedRoomBlockUpdated += OnCurrentSelectedRoomBlockUpdated;
            appState.UI.events.onCurrentSelectedEntityListUpdated += OnCurrentSelectedEntityListUpdated;
        }

        public void SetToolState(ToolState newToolState)
        {
            if (newToolState == toolState) return;

            ToolState previousToolState = toolState;
            TransitionToolState(newToolState, toolState);

            if (events.onToolStateUpdated != null)
            {
                events.onToolStateUpdated(newToolState, previousToolState);
            }
        }

        public void TransitionToolState(ToolState toolState, ToolState previousToolState)
        {
            GetToolState(previousToolState).Teardown();
            this.toolState = toolState;
            GetToolState(toolState).Setup();
        }

        void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            activeToolState.OnSelectionBoxUpdated(selectionBox);
        }

        void OnCurrentSelectedRoomUpdated(Room room)
        {
            activeToolState.OnCurrentSelectedRoomUpdated(room);
        }

        void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock)
        {
            activeToolState.OnCurrentSelectedRoomBlockUpdated(roomBlock);
        }

        void OnSelectionStart(SelectionBox selectionBox)
        {
            activeToolState.OnSelectionStart(selectionBox);
        }

        void OnSelectionEnd(SelectionBox selectionBox)
        {
            activeToolState.OnSelectionEnd(selectionBox);
        }

        void OnCurrentSelectedEntityListUpdated(EntityList entityList)
        {
            activeToolState.OnCurrentSelectedEntityListUpdated(entityList);
        }

        ToolStateBase GetToolState(ToolState toolState)
        {
            if (toolState == ToolState.Build)
            {
                return buildToolState;
            }

            if (toolState == ToolState.Destroy)
            {
                return destroyToolState;
            }

            if (toolState == ToolState.Routes)
            {
                return routesToolState;
            }

            return inspectToolState;
        }
    }
}


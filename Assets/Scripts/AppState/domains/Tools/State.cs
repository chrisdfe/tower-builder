using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using UnityEngine;

namespace TowerBuilder.State.Tools
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

        public class Events
        {
            public delegate void ToolStateUpdatedEvent(ToolState toolState, ToolState previousToolState);
            public ToolStateUpdatedEvent onToolStateUpdated;
        }

        public ToolState toolState { get; private set; } = ToolState.None;

        public NoneToolState noneToolSubState;
        public BuildToolState buildToolSubState;
        public DestroyToolState destroyToolSubState;
        public InspectToolState inspectToolSubState;
        public RoutesToolState routesToolSubState;

        public Events events;

        public State(Input input)
        {
            toolState = input.toolState ?? ToolState.None;

            noneToolSubState = new NoneToolState(this, input.noneToolState);
            buildToolSubState = new BuildToolState(this, input.buildToolState);
            destroyToolSubState = new DestroyToolState(this, input.destroyToolState);
            inspectToolSubState = new InspectToolState(this, input.inspectToolState);
            routesToolSubState = new RoutesToolState(this, input.routesToolState);

            events = new State.Events();
        }

        public State() : this(new Input()) { }

        public void SetToolState(ToolState newToolState)
        {
            if (newToolState == toolState) return;

            TransitionToolState(newToolState, toolState);

            if (events.onToolStateUpdated != null)
            {
                events.onToolStateUpdated(newToolState, toolState);
            }
        }

        public void TransitionToolState(ToolState toolState, ToolState previousToolState)
        {
            Debug.Log("transitioning from " + previousToolState);
            Debug.Log("to " + toolState);
            GetToolSubState(previousToolState).Teardown();
            this.toolState = toolState;
            GetToolSubState(toolState).Setup();
        }

        ToolStateBase GetCurrentActiveToolSubState()
        {
            return GetToolSubState(toolState);
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


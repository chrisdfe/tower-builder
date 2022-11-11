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

        public NoneToolState noneToolState;
        public BuildToolState buildToolState;
        public DestroyToolState destroyToolState;
        public InspectToolState inspectToolState;
        public RoutesToolState routesToolState;

        public Events events;

        public State(Input input)
        {
            toolState = input.toolState ?? ToolState.None;

            noneToolState = new NoneToolState(this, input.noneToolState);
            buildToolState = new BuildToolState(this, input.buildToolState);
            destroyToolState = new DestroyToolState(this, input.destroyToolState);
            inspectToolState = new InspectToolState(this, input.inspectToolState);
            routesToolState = new RoutesToolState(this, input.routesToolState);

            events = new State.Events();
        }

        public State() : this(new Input()) { }

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
            Debug.Log("transitioning from " + previousToolState);
            Debug.Log("to " + toolState);
            GetToolState(previousToolState).Teardown();
            this.toolState = toolState;
            GetToolState(toolState).Setup();
        }

        ToolStateBase GetCurrentActiveToolState()
        {
            return GetToolState(toolState);
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

            if (toolState == ToolState.Inspect)
            {
                return inspectToolState;
            }

            if (toolState == ToolState.Routes)
            {
                return routesToolState;
            }

            return noneToolState;
        }
    }
}


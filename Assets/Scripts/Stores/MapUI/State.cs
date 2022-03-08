using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
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

        public NoneToolState noneToolSubState;
        public BuildToolState buildToolSubState;
        public DestroyToolState destroyToolSubState;
        public InspectToolState inspectToolSubState;

        public ToolStateBase currentActiveToolSubState
        {
            get
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

                return noneToolSubState;
            }
        }

        public State()
        {
            toolState = ToolState.None;
            currentSelectedCell = CellCoordinates.zero;

            noneToolSubState = new NoneToolState(this);
            buildToolSubState = new BuildToolState(this);
            destroyToolSubState = new DestroyToolState(this);
            inspectToolSubState = new InspectToolState(this);
        }

        public void SetToolState(ToolState toolState)
        {
            ToolState previousToolState = this.toolState;
            this.toolState = toolState;

            if (onToolStateUpdated != null)
            {
                onToolStateUpdated(toolState, previousToolState);
            }
        }

        public void SetCurrentSelectedCell(CellCoordinates currentSelectedCell)
        {
            this.currentSelectedCell = currentSelectedCell;

            this.currentActiveToolSubState.OnCurrentSelectedCellSet();

            if (onCurrentSelectedCellUpdated != null)
            {
                onCurrentSelectedCellUpdated(currentSelectedCell);
            }
        }
    }
}


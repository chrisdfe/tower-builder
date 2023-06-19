using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public class State : StateSlice
    {
        public enum Key
        {
            Inspect,
            Build,
            Destroy,
            Routes,
        }

        public static Key DEFAULT_TOOL_STATE = Key.Inspect;

        public struct Input
        {
            public Key? key;
            public CellCoordinates currentSelectedCell;

            public Build.State.Input Build;
            public Destroy.State.Input Destroy;
            public Inspect.State.Input Inspect;
            public Routes.State.Input Routes;
        }

        public delegate void ToolStateUpdatedEvent(Key toolState, Key previousToolState);
        public ToolStateUpdatedEvent onToolStateUpdated;

        public Key currentKey { get; private set; } = DEFAULT_TOOL_STATE;

        public Build.State Build;
        public Destroy.State Destroy;
        public Inspect.State Inspect;
        public Routes.State Routes;

        ToolStateBase activeToolState => GetToolState(currentKey);

        public State(AppState appState, Input input) : base(appState)
        {
            currentKey = input.key ?? DEFAULT_TOOL_STATE;

            Build = new Build.State(appState, this, input.Build);
            Destroy = new Destroy.State(appState, this, input.Destroy);
            Inspect = new Inspect.State(appState, this, input.Inspect);
            Routes = new Routes.State(appState, this, input.Routes);
        }

        public override void Setup()
        {
            base.Setup();

            appState.UI.onSelectionStart += OnSelectionStart;
            appState.UI.onSelectionEnd += OnSelectionEnd;
            appState.UI.onSelectionBoxUpdated += OnSelectionBoxUpdated;
            appState.UI.onSelectionBoxReset += OnSelectionBoxReset;

            appState.UI.onSecondaryActionStart += OnSecondaryActionStart;
            appState.UI.onSecondaryActionEnd += OnSecondaryActionEnd;

            appState.UI.onCurrentSelectedEntityListUpdated += OnCurrentSelectedEntityListUpdated;
            appState.UI.onSelectedEntityBlocksUpdated += OnSelectedEntityBlocksUpdated;
        }

        public override void Teardown()
        {
            base.Teardown();

            appState.UI.onSelectionStart -= OnSelectionStart;
            appState.UI.onSelectionEnd -= OnSelectionEnd;
            appState.UI.onSelectionBoxUpdated -= OnSelectionBoxUpdated;
            appState.UI.onSelectionBoxReset -= OnSelectionBoxReset;

            appState.UI.onSecondaryActionStart -= OnSecondaryActionStart;
            appState.UI.onSecondaryActionEnd -= OnSecondaryActionEnd;

            appState.UI.onCurrentSelectedEntityListUpdated -= OnCurrentSelectedEntityListUpdated;
            appState.UI.onSelectedEntityBlocksUpdated -= OnSelectedEntityBlocksUpdated;
        }

        /*
            Public Interface
        */
        public void SetToolState(Key newToolStateKey)
        {
            if (newToolStateKey != currentKey)
            {
                Key previousToolState = newToolStateKey;
                TransitionToolState(newToolStateKey, currentKey);

                onToolStateUpdated?.Invoke(newToolStateKey, previousToolState);
            }
        }

        public void TransitionToolState(Key currentKey, Key previousKey)
        {
            GetToolState(previousKey).Teardown();
            this.currentKey = currentKey;
            GetToolState(currentKey).Setup();
        }

        /*
            Event Handlers
        */
        void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            activeToolState.OnSelectionBoxUpdated(selectionBox);
        }

        void OnCurrentSelectedRoomUpdated(Room room)
        {
            activeToolState.OnCurrentSelectedRoomUpdated(room);
        }

        void OnSelectionStart(SelectionBox selectionBox)
        {
            activeToolState.OnSelectionStart(selectionBox);
        }

        void OnSelectionEnd(SelectionBox selectionBox)
        {
            activeToolState.OnSelectionEnd(selectionBox);
        }

        void OnSelectionBoxReset(SelectionBox selectionBox)
        {
            activeToolState.OnSelectionBoxReset(selectionBox);
        }

        void OnCurrentSelectedEntityListUpdated(ListWrapper<Entity> entityList)
        {
            activeToolState.OnCurrentSelectedEntityListUpdated(entityList);
        }

        void OnSelectedEntityBlocksUpdated(CellCoordinatesBlockList selectedBlocksList)
        {
            activeToolState.OnSelectedEntityBlocksUpdated(selectedBlocksList);
        }

        void OnSecondaryActionStart()
        {
            activeToolState.OnSecondaryActionStart();
        }

        void OnSecondaryActionEnd()
        {
            activeToolState.OnSecondaryActionEnd();
        }

        /*
            Internals
        */
        ToolStateBase GetToolState(Key key) =>
            key switch
            {
                Key.Build => Build,
                Key.Destroy => Destroy,
                Key.Routes => Routes,
                Key.Inspect => Inspect,
                _ => throw new NotSupportedException("Unsupported tool state: " + key)
            };
    }
}



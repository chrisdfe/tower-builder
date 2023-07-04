using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.Definitions;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools.Build
{
    public class State : ToolStateBase
    {
        public enum Mode
        {
            Entities,
            Rooms
        }

        public struct Input
        {
            public Entities.State.Input Entities;
            public Rooms.State.Input Rooms;
        }

        public delegate void ModeEvent(Mode newMode, Mode previousMode);
        public ModeEvent onModeUpdated;

        public delegate void BuildIsActiveEvent();
        public BuildIsActiveEvent onBuildStart;
        public BuildIsActiveEvent onBuildEnd;

        public Entities.State Entities;
        public Rooms.State Rooms;

        public Mode currentMode { get; private set; } = Mode.Entities;

        public bool buildIsActive { get; private set; } = false;
        bool isLocked = false;

        public State(AppState appState, Input input) : base(appState)
        {
            Entities = new Entities.State(appState, input.Entities);
            Rooms = new Rooms.State(appState, input.Rooms);
        }

        public override void Setup()
        {
            base.Setup();

            GetCurrentMode().Setup();
        }

        public override void Teardown()
        {
            base.Teardown();

            GetCurrentMode().Teardown();
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            StartBuild();
        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {
            EndBuild();
        }

        public override void OnSecondaryActionEnd()
        {
            base.OnSecondaryActionEnd();
            appState.Tools.SetToolState(ApplicationState.Tools.State.Key.Inspect);
        }

        // Pass through to child
        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            GetCurrentMode().OnSelectionBoxUpdated(selectionBox);
        }

        public override void OnSelectionBoxReset(SelectionBox selectionBox)
        {
            GetCurrentMode().OnSelectionBoxReset(selectionBox);
        }

        public override void OnEntitiesInSelectionUpdated(List<Entity> entityList)
        {
            GetCurrentMode().OnEntitiesInSelectionUpdated(entityList);
        }

        public override void OnEntityBlocksInSelectionUpdated(List<CellCoordinatesBlock> roomBlockList)
        {
            GetCurrentMode().OnEntityBlocksInSelectionUpdated(roomBlockList);
        }

        public void SetMode(Mode newMode)
        {
            if (newMode == currentMode) return;

            Mode previousMode = currentMode;
            TransitionToolState(newMode, previousMode);

            onModeUpdated?.Invoke(newMode, previousMode);
        }

        /* 
            Internals
        */
        void TransitionToolState(Mode newMode, Mode previousMode)
        {
            GetMode(previousMode).Teardown();
            this.currentMode = newMode;
            GetMode(currentMode).Setup();
        }

        BuildModeStateBase GetMode(Mode mode) =>
            mode switch
            {
                Mode.Entities => Entities,
                Mode.Rooms => Rooms,
                _ => throw new NotSupportedException("Unsupported build tool mode: " + mode)
            };

        BuildModeStateBase GetCurrentMode() => GetMode(currentMode);

        void StartBuild()
        {
            if (isLocked) return;

            buildIsActive = true;

            GetCurrentMode().OnBuildStart();

            onBuildStart?.Invoke();
        }

        void EndBuild()
        {
            // EndBuild can be called when the mouse click up event happens outside the screen or over a UI element
            if (isLocked || !buildIsActive) return;

            isLocked = true;

            GetCurrentMode().OnBuildEnd();

            onBuildEnd?.Invoke();

            isLocked = false;
            buildIsActive = false;
        }
    }
}
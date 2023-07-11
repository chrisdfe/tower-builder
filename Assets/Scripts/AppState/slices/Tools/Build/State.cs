using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups;
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

        public delegate void BlueprintEvent(EntityGroup blueprint);
        public BlueprintEvent onBlueprintUpdated;

        public Entities.State Entities;
        public Rooms.State Rooms;

        public Mode currentMode { get; private set; } = Mode.Entities;

        public bool buildIsActive { get; private set; } = false;
        bool isLocked = false;

        public EntityGroup blueprint = new EntityGroup();

        List<BuildModeStateBase> allModeHandlers;

        public State(AppState appState, Input input) : base(appState)
        {
            Entities = new Entities.State(appState, input.Entities);
            Rooms = new Rooms.State(appState, input.Rooms);

            allModeHandlers = new List<BuildModeStateBase>() {
                Entities,
                Rooms
            };
        }

        public override void Setup()
        {
            base.Setup();

            CreateBlueprint();

            allModeHandlers.ForEach((buildModeStateBase) =>
            {
                buildModeStateBase.onResetRequested += ResetBlueprint;
            });

            GetCurrentMode().Setup();
        }

        public override void Teardown()
        {
            base.Teardown();

            RemoveBlueprint();

            allModeHandlers.ForEach((buildModeStateBase) =>
            {
                buildModeStateBase.onResetRequested -= ResetBlueprint;
            });

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

        public override void OnSelectionBoxReset(SelectionBox selectionBox)
        {
            base.OnSelectionBoxReset(selectionBox);

            ResetBlueprint();
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            base.OnSelectionBoxUpdated(selectionBox);

            if (appState.Tools.Build.buildIsActive)
            {
                ResetBlueprint();
            }
            else
            {
                // appState.EntityGroups.UpdateOffsetCoordinates(blueprintRoom, selectionBox.cellCoordinatesList.bottomLeftCoordinates);

                // TODO - reimplement updating entity/entity group coordinates again
                // Reset for now
                ResetBlueprint();

                // onBlueprintPositionUpdated?.Invoke(blueprintRoom);
            }

            onBlueprintUpdated?.Invoke(blueprint);
        }

        public override void OnSecondaryActionEnd()
        {
            base.OnSecondaryActionEnd();
            appState.Tools.SetToolState(ApplicationState.Tools.State.Key.Inspect);
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
            BuildModeStateBase currentMode = GetCurrentMode();

            if (currentMode != null)
            {
                currentMode.Teardown();
            }

            this.currentMode = newMode;

            GetCurrentMode().Setup();
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

            onBuildStart?.Invoke();
        }

        void EndBuild()
        {
            // EndBuild can be called when the mouse click up event happens outside the screen or over a UI element
            if (isLocked || !buildIsActive) return;

            isLocked = true;

            blueprint.buildValidator.ValidateWithChildren(appState);

            if (blueprint.buildValidator.GetAllValidationErrors().Count == 0)
            {
                BuildBlueprint();
                CreateBlueprint();
            }
            else
            {
                appState.Notifications.Add(blueprint.buildValidator.GetAllValidationErrors());
                ResetBlueprint();
            }

            onBuildEnd?.Invoke();

            isLocked = false;
            buildIsActive = false;
        }

        void ResetBlueprint()
        {
            RemoveBlueprint();
            CreateBlueprint();
        }

        void CreateBlueprint()
        {
            blueprint = GetCurrentMode().CalculateBlueprintEntityGroup();
            blueprint.SetBlueprintMode(true);
            blueprint.buildValidator.ValidateWithChildren(appState);
            blueprint.relativeOffsetCoordinates = appState.UI.selectionBox.cellCoordinatesList.bottomLeftCoordinates;
            appState.EntityGroups.AddWithChildren(blueprint);
        }

        void RemoveBlueprint()
        {
            if (blueprint == null) return;

            appState.EntityGroups.RemoveWithChildren(blueprint);
        }

        void BuildBlueprint()
        {
            blueprint.UpdateChildrenBeforeParentRemove();
            appState.EntityGroups.Remove(blueprint);
            appState.EntityGroups.Build(blueprint.GetDescendantEntityGroups());
            appState.Entities.Build(blueprint.GetDescendantEntities());
        }
    }
}
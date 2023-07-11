using System;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools.Destroy
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

        public delegate void DestroyEvent();
        public DestroyEvent onDestroyStart;
        public DestroyEvent onDestroyEnd;
        public DestroyEvent onDestroySelectionUpdated;

        public ListWrapper<Entity> entitiesToDelete { get; private set; } = new ListWrapper<Entity>();

        public bool destroyIsActive { get; private set; } = false;
        bool isLocked = false;

        public State(AppState appState, Input input) : base(appState)
        {
            Entities = new Entities.State(appState, input.Entities);
            Rooms = new Rooms.State(appState, input.Rooms);
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            entitiesToDelete = GetCurrentMode().CalculateEntitiesToDelete();
            onDestroySelectionUpdated?.Invoke();
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            StartDestroy();
        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {
            EndDestroy();
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
            GetMode(previousMode).Teardown();
            this.currentMode = newMode;
            GetMode(currentMode).Setup();
        }

        void StartDestroy()
        {
            if (isLocked) return;

            destroyIsActive = true;

            UnmarkCurrentEntitiesMarkedForDeletion();
            entitiesToDelete = GetCurrentMode().CalculateEntitiesToDelete();
            ValidateAndMarkEntitiesToDelete();

            onDestroyStart?.Invoke();
        }

        void EndDestroy()
        {
            // This happens when the mouse click up happens outside the screen or over a UI element
            if (!destroyIsActive || isLocked) return;

            ValidateAndMarkEntitiesToDelete();

            ListWrapper<ValidationError> validationErrors = GetAllValidationErrors();
            if (validationErrors.Count == 0)
            {
                DeleteEntitiesMarkedForDeletion();
            }
            else
            {
                appState.Notifications.Add(validationErrors);
            }

            destroyIsActive = false;

            onDestroyEnd?.Invoke();
        }

        void ValidateAndMarkEntitiesToDelete()
        {
            foreach (Entity entity in entitiesToDelete.items)
            {
                entity.destroyValidator.Validate(appState);
                entity.isMarkedForDeletion = true;
            }
        }

        void DeleteEntitiesMarkedForDeletion()
        {
            foreach (Entity entity in entitiesToDelete.items)
            {
                // TODO - just pass entitiesToDelete in here as list instead
                appState.Entities.Remove(entity);
            }
        }

        void UnmarkCurrentEntitiesMarkedForDeletion()
        {
            entitiesToDelete.ForEach(entity =>
            {
                entity.isMarkedForDeletion = false;
            });
        }

        ListWrapper<ValidationError> GetAllValidationErrors() =>
            entitiesToDelete.items
                .Aggregate(
                    new ListWrapper<ValidationError>(),
                    (acc, entity) =>
                    {
                        entity.destroyValidator.Validate(appState);
                        acc.Add(entity.destroyValidator.errors);
                        return acc;
                    }
                );

        DestroyModeStateBase GetMode(Mode mode) =>
            mode switch
            {
                Mode.Entities => Entities,
                Mode.Rooms => Rooms,
                _ => throw new NotSupportedException("Unsupported destroy tool mode: " + mode)
            };

        DestroyModeStateBase GetCurrentMode() => GetMode(currentMode);
    }
}
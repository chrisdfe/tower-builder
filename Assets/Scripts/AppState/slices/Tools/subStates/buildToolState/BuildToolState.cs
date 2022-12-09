using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.Definitions;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public struct Input
        {
            public string selectedRoomCategory;
            public RoomDefinition selectedRoomDefinition;
        }

        public class Events
        {
            public delegate void SelectedEntityKeyEvent(Entity.Type entityKey, Entity.Type previousEntityType);
            public SelectedEntityKeyEvent onSelectedEntityKeyUpdated;

            public delegate void buildIsActiveEvent();
            public buildIsActiveEvent onBuildStart;
            public buildIsActiveEvent onBuildEnd;
        }

        public Entity.Type selectedEntityKey { get; private set; } = Entity.Type.Room;

        public BuildToolState.Events events;

        public bool isLocked = false;
        bool buildIsActive = false;

        public class SubStates
        {
            public RoomEntityTypeSubState roomEntityType;
            // public FurnitureEntityTypeSubState furnitureEntityType;
            // public ResidentEntityTypeSubState residentEntityType;
            // public TransportationItemEntityTypeSubState transportationItemEntityType;

            public SubStates(BuildToolState buildToolState)
            {
                roomEntityType = new RoomEntityTypeSubState(buildToolState);
                // furnitureEntityType = new FurnitureEntityTypeSubState(buildToolState);
                // residentEntityType = new ResidentEntityTypeSubState(buildToolState);
                // transportationItemEntityType = new TransportationItemEntityTypeSubState(buildToolState);
            }
        }

        public SubStates subStates { get; private set; }

        public EntityTypeSubState currentSubState { get => GetSubState(selectedEntityKey); }

        public BuildToolState(AppState appState, Tools.State state, Input input) : base(appState, state)
        {
            events = new Events();

            subStates = new SubStates(this);
        }

        public override void Setup()
        {
            base.Setup();
            currentSubState.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();
            currentSubState.Teardown();
        }

        public override void OnSelectionStart(SelectionBox selectionBox)
        {
            StartBuild();
        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {
            EndBuild();
        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            if (isLocked) return;

            currentSubState.OnSelectionBoxUpdated();
        }

        public void SetSelectedEntityKey(Entity.Type entityType)
        {
            isLocked = true;
            Entity.Type previousEntityType = this.selectedEntityKey;
            this.selectedEntityKey = entityType;

            // tear previous state down
            GetSubState(previousEntityType).Teardown();

            // set new state up
            GetSubState(this.selectedEntityKey).Setup();

            isLocked = false;

            events.onSelectedEntityKeyUpdated?.Invoke(this.selectedEntityKey, previousEntityType);
        }

        void StartBuild()
        {
            if (isLocked) return;
            buildIsActive = true;

            currentSubState.StartBuild();

            if (events.onBuildStart != null)
            {
                events.onBuildStart();
            }
        }

        void EndBuild()
        {
            // EndBuild can be called when the mouse click up event happens outside the screen or over a UI element
            if (isLocked || !buildIsActive) return;
            buildIsActive = false;

            currentSubState.EndBuild();

            if (events.onBuildEnd != null)
            {
                events.onBuildEnd();
            }
        }

        EntityTypeSubState GetSubState(Entity.Type entityType) =>
            entityType switch
            {
                Entity.Type.Room => subStates.roomEntityType,
                // Entity.Type.Furniture => subStates.furnitureEntityType,
                // Entity.Type.Resident => subStates.residentEntityType,
                // Entity.Type.TransportationItem => subStates.transportationItemEntityType,
                _ => throw new NotImplementedException("invalid entity type: " + entityType)
            };
    }
}
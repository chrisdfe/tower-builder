using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.Utils;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public struct Input
        {
            public string selectedRoomCategory;
            public RoomTemplate selectedRoomTemplate;
        }

        public class Events
        {
            public delegate void SelectedEntityTypeEvent(EntityType entityType, EntityType previousEntityType);
            public SelectedEntityTypeEvent onSelectedEntityTypeUpdated;

            public delegate void buildIsActiveEvent();
            public buildIsActiveEvent onBuildStart;
            public buildIsActiveEvent onBuildEnd;
        }

        public EntityType selectedEntityType { get; private set; } = EntityType.Room;

        public BuildToolState.Events events;

        public bool isLocked = false;
        bool buildIsActive = false;

        public class SubStates
        {
            public RoomEntityTypeSubState roomEntityType;
            public FurnitureEntityTypeSubState furnitureEntityType;
            public ResidentEntityTypeSubState residentEntityType;
            public TransportationItemEntityTypeSubState transportationItemEntityType;

            public SubStates(BuildToolState buildToolState)
            {
                roomEntityType = new RoomEntityTypeSubState(buildToolState);
                furnitureEntityType = new FurnitureEntityTypeSubState(buildToolState);
                residentEntityType = new ResidentEntityTypeSubState(buildToolState);
                transportationItemEntityType = new TransportationItemEntityTypeSubState(buildToolState);
            }
        }

        public SubStates subStates { get; private set; }

        public EntityTypeSubState currentSubState { get { return GetSubState(selectedEntityType); } }

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

        public void SetSelectedEntityType(EntityType entityType)
        {
            isLocked = true;
            EntityType previousEntityType = this.selectedEntityType;
            this.selectedEntityType = entityType;

            // tear previous state down
            GetSubState(previousEntityType).Teardown();

            // set new state up
            GetSubState(this.selectedEntityType).Setup();

            isLocked = false;

            if (events.onSelectedEntityTypeUpdated != null)
            {
                events.onSelectedEntityTypeUpdated(this.selectedEntityType, previousEntityType);
            }
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


        EntityTypeSubState GetSubState(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Room:
                    return subStates.roomEntityType;
                case EntityType.Furniture:
                    return subStates.furnitureEntityType;
                case EntityType.Resident:
                    return subStates.residentEntityType;
                case EntityType.TransportationItem:
                    return subStates.transportationItemEntityType;
                default:
                    throw new NotImplementedException("invalid entity type: " + entityType);
            }
        }
    }
}
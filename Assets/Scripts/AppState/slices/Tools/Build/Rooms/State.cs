using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools.Build.Rooms
{
    public class State : BuildModeStateBase
    {
        public struct Input { }

        public class Events
        {
            public delegate void RoomKeyEvent(string roomKey);
            public RoomKeyEvent onRoomKeyUpdated;

            public delegate void blueprintUpdateEvent(Room blueprintRoom);
            public blueprintUpdateEvent onBlueprintRoomUpdated;
        }

        public Events events { get; private set; }

        public string selectedRoomKey { get; private set; } = "Office";

        public RoomDefinition selectedRoomDefinition { get; private set; } = null;

        public Room blueprintRoom { get; private set; } = null;

        public State(AppState appState, Tools.State state, Build.State buildState, Input input) : base(appState, state, buildState)
        {
            events = new Events();
        }

        /*
            Public Interface
        */
        public void SetSelectedRoomKey(string value)
        {
            this.selectedRoomKey = value;

            ResetDefinition();
            ResetBlueprintRoom();

            events.onRoomKeyUpdated?.Invoke(this.selectedRoomKey);
        }

        /*
            Lifecycle
        */
        public override void Setup()
        {
            base.Setup();

            ResetDefinition();
            ResetBlueprintRoom();
        }

        public override void Teardown()
        {
            base.Teardown();

            RemoveBlueprintRoom();
        }

        /*
            Inherited event handlers
        */
        public override void OnSelectionStart(SelectionBox selectionBox)
        {

        }

        public override void OnSelectionEnd(SelectionBox selectionBox)
        {

        }

        public override void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            if (isLocked) return;

            ResetBlueprintRoom();

            events.onBlueprintRoomUpdated?.Invoke(blueprintRoom);
        }

        public override void OnBuildStart()
        {
            base.OnBuildStart();
        }

        public override void OnBuildEnd()
        {
            base.OnBuildEnd();

            blueprintRoom.Validate(Registry.appState);

            if (blueprintRoom.isValid)
            {
                BuildBlueprintRoom();
                CreateBlueprintRoom();
            }
            else
            {
                Registry.appState.Notifications.Add(
                    new ListWrapper<Notification>(
                        blueprintRoom.validationErrors.items
                            .Select(error => new Notification(error.message))
                            .ToList()
                    )
                );

                ResetBlueprintRoom();
            }
        }

        /*
            Internals
        */
        void ResetDefinition()
        {
            selectedRoomDefinition = DataTypes.EntityGroups.Definitions.Rooms.FindByKey(selectedRoomKey) as RoomDefinition;
        }

        void CreateBlueprintRoom()
        {
            RoomBuilderBase roomBuilder = selectedRoomDefinition.builderFactory(selectedRoomDefinition) as RoomBuilderBase;
            blueprintRoom = roomBuilder.Build(Registry.appState.UI.selectionBox) as Room;

            blueprintRoom.SetBlueprintMode(true);

            Registry.appState.EntityGroups.Rooms.Add(blueprintRoom);

            blueprintRoom.Validate(appState);
        }

        void BuildBlueprintRoom()
        {
            Registry.appState.EntityGroups.Rooms.Build(blueprintRoom);

            blueprintRoom = null;
        }

        void RemoveBlueprintRoom()
        {
            if (blueprintRoom == null) return;

            Registry.appState.EntityGroups.Rooms.Remove(blueprintRoom);

            blueprintRoom = null;
        }

        void ResetBlueprintRoom()
        {
            RemoveBlueprintRoom();
            CreateBlueprintRoom();
        }
    }
}
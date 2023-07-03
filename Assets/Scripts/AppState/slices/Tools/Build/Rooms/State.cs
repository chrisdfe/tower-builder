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

        public delegate void RoomKeyEvent(string roomKey);
        public RoomKeyEvent onRoomKeyUpdated;

        public delegate void BlueprintEvent(Room blueprintRoom);
        public BlueprintEvent onBlueprintUpdated;
        public BlueprintEvent onBlueprintPositionUpdated;

        public string selectedRoomKey { get; private set; } = "Office";

        public RoomDefinition selectedRoomDefinition { get; private set; } = null;

        public Room blueprintRoom { get; private set; } = null;

        public State(AppState appState, Tools.State state, Build.State buildState, Input input) : base(appState, state, buildState)
        {
        }

        /*
            Public Interface
        */
        public void SetSelectedRoomKey(string value)
        {
            this.selectedRoomKey = value;

            ResetDefinition();
            ResetBlueprintRoom();

            onRoomKeyUpdated?.Invoke(this.selectedRoomKey);
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


            if (buildState.buildIsActive)
            {
                ResetBlueprintRoom();
            }
            else
            {
                appState.EntityGroups.UpdateOffsetCoordinates(blueprintRoom, selectionBox.cellCoordinatesList.bottomLeftCoordinates);

                onBlueprintUpdated?.Invoke(blueprintRoom);
                onBlueprintPositionUpdated?.Invoke(blueprintRoom);
            }
        }

        public override void OnBuildStart()
        {
            base.OnBuildStart();
        }

        public override void OnBuildEnd()
        {
            base.OnBuildEnd();

            blueprintRoom.ValidateBuild(Registry.appState);

            if (blueprintRoom.canBuild)
            {
                BuildBlueprintRoom();
                CreateBlueprintRoom();
            }
            else
            {
                Registry.appState.Notifications.Add(blueprintRoom.buildValidationErrors);
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
            blueprintRoom.offsetCoordinates = Registry.appState.UI.selectionBox.cellCoordinatesList.bottomLeftCoordinates;
            blueprintRoom.ValidateBuild(appState);

            Registry.appState.EntityGroups.Rooms.Add(blueprintRoom);
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
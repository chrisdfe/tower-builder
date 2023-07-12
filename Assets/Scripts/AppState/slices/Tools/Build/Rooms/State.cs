using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
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

        public string selectedRoomKey { get; private set; } = "Office";

        public RoomDefinition selectedRoomDefinition { get; private set; } = null;

        public State(AppState appState, Input input) : base(appState) { }

        public override void Setup()
        {
            base.Setup();

            ResetDefinition();
        }
        /*
            Public Interface
        */
        public override Blueprint CalculateBlueprint()
        {
            Blueprint blueprint = new Blueprint();

            EntityGroup room = EntityGroup.CreateFromDefinition(selectedRoomDefinition, appState);
            blueprint.Add(room);

            return blueprint;
        }

        public void SetSelectedRoomKey(string value)
        {
            this.selectedRoomKey = value;
            ResetDefinition();

            onResetRequested?.Invoke();
            onRoomKeyUpdated?.Invoke(this.selectedRoomKey);
        }

        /*
            Internals
        */
        void ResetDefinition()
        {
            selectedRoomDefinition = DataTypes.EntityGroups.Definitions.Rooms.FindByKey(selectedRoomKey) as RoomDefinition;
        }
    }
}
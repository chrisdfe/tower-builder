using System;
using TowerBuilder;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class BuildToolStateInputHandlers : ToolStateInputHandlersBase
    {
        public BuildToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager)
        {
        }

        public override void Update() { }

        public override void OnTransitionTo(ToolState newToolState)
        {
            // CreateBlueprint();
            // Registry.appState.Rooms.events.onRoomAdded += OnRoomAdded;
        }

        public override void OnTransitionFrom(ToolState previousToolState)
        {
            // DestroyBlueprint();
            // Registry.appState.Rooms.events.onRoomAdded -= OnRoomAdded;
        }

        public override void OnMouseDown()
        {
            // Registry.appState.Tools.buildToolState.StartBuild();
        }

        public override void OnMouseUp()
        {
            // Registry.appState.Tools.buildToolState.EndBuild();
        }

        void OnRoomAdded(Room room)
        {
            // if (room.id == Registry.appState.Tools.buildToolState.currentBlueprint.room.id)
            // {
            //     ResetBlueprint();
            // }
        }
    }
}
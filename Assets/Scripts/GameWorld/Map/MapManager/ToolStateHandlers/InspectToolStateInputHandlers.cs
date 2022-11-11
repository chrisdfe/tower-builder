using System;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State;
using TowerBuilder.State.Tools;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class InspectToolStateInputHandlers : ToolStateInputHandlersBase
    {
        Room currentSelectedRoom;
        Room currentInspectedRoom;

        public InspectToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager) { }

        public override void OnTransitionTo(ToolState previousState) { }

        public override void OnTransitionFrom(ToolState previousState) { }

        public override void OnMouseUp()
        {
            Registry.appState.Tools.inspectToolState.InspectCurrentSelectedRoom();
        }
    }
}
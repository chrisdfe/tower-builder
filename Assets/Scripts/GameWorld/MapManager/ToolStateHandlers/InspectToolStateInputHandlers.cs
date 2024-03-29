using System;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class InspectToolStateInputHandlers : ToolStateInputHandlersBase
    {
        Room currentSelectedRoom;
        Room currentInspectedRoom;

        public InspectToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager) { }

        public override void OnTransitionTo(ApplicationState.Tools.State.Key previousState) { }

        public override void OnTransitionFrom(ApplicationState.Tools.State.Key previousState) { }

        public override void OnMouseUp()
        {
            // Registry.appState.Tools.inspectToolState.InspectCurrentSelectedRoom();
        }
    }
}
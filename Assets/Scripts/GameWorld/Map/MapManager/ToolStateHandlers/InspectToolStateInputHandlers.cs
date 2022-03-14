using System;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
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
            Registry.Stores.MapUI.inspectToolSubState.InspectCurrentSelectedRoom();
        }
    }
}
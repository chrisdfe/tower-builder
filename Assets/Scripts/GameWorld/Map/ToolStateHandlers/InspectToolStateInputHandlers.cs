using System;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map
{
    public class InspectToolStateInputHandlers : ToolStateInputHandlersBase
    {
        Room currentSelectedRoom;
        Room currentInspectedRoom;

        public InspectToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager) { }

        public override void OnTransitionTo(ToolState previousState) { }

        public override void OnTransitionFrom(ToolState previousState)
        {
            // TODO - do this in the store instead
            Registry.Stores.MapUI.inspectToolSubState.Reset();

        }

        public override void OnMouseUp()
        {
            Room currentSelectedRoom = Registry.Stores.MapUI.inspectToolSubState.currentSelectedRoom;
            Registry.Stores.MapUI.inspectToolSubState.AttemptToInspectRoom(currentSelectedRoom);
        }
    }
}
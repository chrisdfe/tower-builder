using System;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
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
            Registry.Stores.UI.inspectToolSubState.InspectCurrentSelectedRoom();
        }
    }
}
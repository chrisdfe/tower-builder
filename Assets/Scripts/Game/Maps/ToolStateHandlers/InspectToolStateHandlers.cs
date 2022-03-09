using System;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public class InspectToolStateHandlers : ToolStateHandlersBase
    {
        Room currentSelectedRoom;
        Room currentInspectedRoom;

        public InspectToolStateHandlers(MapManager parentMapManager) : base(parentMapManager)
        {
        }

        public override void OnTransitionTo(ToolState previousState)
        {
            Debug.Log("Inspect mode start");
        }

        public override void OnTransitionFrom(ToolState previousState)
        {
            Debug.Log("Inspect mode end");
            Registry.Stores.MapUI.inspectToolSubState.Reset();

        }

        public override void OnMouseUp()
        {
            Room currentSelectedRoom = Registry.Stores.MapUI.inspectToolSubState.currentSelectedRoom;
            Registry.Stores.MapUI.inspectToolSubState.AttemptToInspectRoom(currentSelectedRoom);
        }
    }
}
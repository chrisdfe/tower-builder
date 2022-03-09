using System;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Game.Maps
{
    public class DestroyToolStateHandlers : ToolStateHandlersBase
    {
        public DestroyToolStateHandlers(MapManager parentMapManager) : base(parentMapManager) { }

        public override void OnTransitionTo(ToolState previousToolState)
        {
        }

        public override void OnTransitionFrom(ToolState nextToolState)
        {
            Registry.Stores.MapUI.destroyToolSubState.Reset();
        }

        public override void OnMouseUp()
        {
            Room currentSelectedRoom = Registry.Stores.MapUI.destroyToolSubState.currentSelectedRoom;
            Registry.Stores.MapUI.destroyToolSubState.AttemptToDestroyRoom(currentSelectedRoom);
        }
    }
}


using System;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map
{
    public class DestroyToolStateInputHandlers : ToolStateInputHandlersBase
    {
        public DestroyToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager) { }

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


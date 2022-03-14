using System;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class DestroyToolStateInputHandlers : ToolStateInputHandlersBase
    {
        public DestroyToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager) { }

        public override void OnTransitionTo(ToolState previousToolState) { }

        public override void OnTransitionFrom(ToolState nextToolState) { }

        public override void OnMouseUp()
        {
            Registry.Stores.MapUI.destroyToolSubState.DestroyCurrentSelectedRoom();
        }
    }
}


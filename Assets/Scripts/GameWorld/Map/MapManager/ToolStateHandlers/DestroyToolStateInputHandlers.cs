using System;
using TowerBuilder.DataTypes;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
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
            Registry.appState.UI.destroyToolSubState.DestroyCurrentSelectedRoomBlock();
        }
    }
}


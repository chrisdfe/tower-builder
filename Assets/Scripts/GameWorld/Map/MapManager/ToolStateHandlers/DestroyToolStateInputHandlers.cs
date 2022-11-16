using System;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
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
            // Registry.appState.Tools.destroyToolState.DestroyCurrentSelectedRoomBlock();
        }
    }
}


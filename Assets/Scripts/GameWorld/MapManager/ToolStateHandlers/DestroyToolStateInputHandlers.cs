using System;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Tools;
using TowerBuilder.DataTypes;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class DestroyToolStateInputHandlers : ToolStateInputHandlersBase
    {
        public DestroyToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager) { }

        public override void OnTransitionTo(ApplicationState.Tools.State.Key previousToolState) { }

        public override void OnTransitionFrom(ApplicationState.Tools.State.Key nextToolState) { }

        public override void OnMouseUp()
        {
            // Registry.appState.Tools.destroyToolState.DestroyCurrentSelectedRoomBlock();
        }
    }
}


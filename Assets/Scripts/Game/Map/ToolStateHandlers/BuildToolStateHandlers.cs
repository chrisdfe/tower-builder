using System;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class BuildToolStateHandlers : ToolStateHandlersBase
    {
        public BuildToolStateHandlers(MapManager parentMapManager) : base(parentMapManager) { }

        public override void Update() { }

        public override void OnTransitionTo(ToolState previousToolState)
        {
            mapManager.mapCursor.Enable();
        }

        public override void OnTransitionFrom(ToolState nextToolState)
        {
            mapManager.mapCursor.Disable();
        }

        public override void OnMouseDown()
        {
            Registry.Stores.MapUI.StartBuild();
        }

        public override void OnMouseUp()
        {
            Registry.Stores.MapUI.EndBuild();
        }
    }
}
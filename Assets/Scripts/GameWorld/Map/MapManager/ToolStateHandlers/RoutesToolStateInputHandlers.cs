using System;
using TowerBuilder;
using TowerBuilder.GameWorld.Rooms.Blueprints;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;

namespace TowerBuilder.GameWorld.Map.MapManager
{
    public class RoutesToolStateInputHandlers : ToolStateInputHandlersBase
    {

        public RoutesToolStateInputHandlers(GameWorldMapManager parentMapManager) : base(parentMapManager)
        {
        }

        public override void Update() { }

        public override void OnTransitionTo(ToolState previousToolState)
        {
            Debug.Log("Hello I am routes state");
        }

        public override void OnTransitionFrom(ToolState nextToolState)
        {
            Debug.Log("Goodbye from routes state");
        }

        public override void OnMouseUp()
        {
            CellCoordinates currentCell = Registry.Stores.UI.currentSelectedCell;
            switch (Registry.Stores.UI.routesToolSubState.currentClickState)
            {
                case RoutesToolState.ClickState.RouteStart:
                    Registry.Stores.Routes.SetDebugRouteStart(currentCell);
                    break;
                case RoutesToolState.ClickState.RouteEnd:
                    Registry.Stores.Routes.SetDebugRouteEnd(currentCell);
                    break;
                default:
                    break;
            }
        }
    }
}
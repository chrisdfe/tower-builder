using System;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State;
using TowerBuilder.State.Tools;
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
            CellCoordinates currentCell = Registry.appState.UI.currentSelectedCell;
            switch (Registry.appState.Tools.routesToolState.currentClickState)
            {
                case RoutesToolState.ClickState.RouteStart:
                    Registry.appState.Routes.SetDebugRouteStart(currentCell);
                    break;
                case RoutesToolState.ClickState.RouteEnd:
                    Registry.appState.Routes.SetDebugRouteEnd(currentCell);
                    break;
                default:
                    break;
            }
        }
    }
}
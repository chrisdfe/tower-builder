using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class RoutesToolState : ToolStateBase
    {
        public enum ClickState
        {
            None,
            RouteStart,
            RouteEnd,
        }

        public ClickState currentClickState { get; private set; } = ClickState.None;

        public RoutesToolState(MapUI.State state) : base(state) { }

        public override void Setup() { }

        public override void Teardown() { }

        public void SetClickState(ClickState clickState)
        {
            this.currentClickState = clickState;
        }
    }
}
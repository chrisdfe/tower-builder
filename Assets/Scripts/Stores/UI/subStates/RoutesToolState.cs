using System.Collections.Generic;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;

namespace TowerBuilder.Stores.UI
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

        public RoutesToolState(UI.State state) : base(state) { }

        public override void Setup() { }

        public override void Teardown() { }

        public void SetClickState(ClickState clickState)
        {
            this.currentClickState = clickState;
        }
    }
}
using System.Collections.Generic;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;

namespace TowerBuilder.Stores.UI
{
    public class RoutesToolState : ToolStateBase
    {
        public struct Input
        {
            public ClickState? currentClickState;
        }

        public enum ClickState
        {
            None,
            RouteStart,
            RouteEnd,
        }

        public ClickState currentClickState { get; private set; }

        public RoutesToolState(UI.State state, Input input) : base(state)
        {
            currentClickState = input.currentClickState ?? ClickState.None;
        }

        public override void Setup() { }

        public override void Teardown() { }

        public void SetClickState(ClickState clickState)
        {
            this.currentClickState = clickState;
        }
    }
}
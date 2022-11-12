using System.Collections.Generic;
using TowerBuilder.State;

using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.State.Tools
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

        public RoutesToolState(AppState appState, Tools.State state, Input input) : base(appState, state)
        {
            currentClickState = input.currentClickState ?? ClickState.None;
        }

        public void SetClickState(ClickState clickState)
        {
            this.currentClickState = clickState;
        }
    }
}
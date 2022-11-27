using System.Collections.Generic;
using TowerBuilder.ApplicationState;

using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.ApplicationState.UI;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Tools
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
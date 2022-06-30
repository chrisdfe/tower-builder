using System.Collections.Generic;
using TowerBuilder.State;

using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.State.UI
{
    public class DestroyToolState : ToolStateBase
    {
        public struct Input { }

        public DestroyToolState(UI.State state, Input input) : base(state) { }

        public void DestroyCurrentSelectedRoomBlock()
        {
            Registry.appState.Rooms.DestroyRoomBlock(parentState.currentSelectedRoom, parentState.currentSelectedRoomBlock);
        }
    }
}
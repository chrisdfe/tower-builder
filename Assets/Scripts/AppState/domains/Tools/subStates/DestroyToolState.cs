using System.Collections.Generic;
using TowerBuilder.State;

using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.State.Tools
{
    public class DestroyToolState : ToolStateBase
    {
        public struct Input { }

        public DestroyToolState(Tools.State state, Input input) : base(state) { }

        public void DestroyCurrentSelectedRoomBlock()
        {
            Registry.appState.Rooms.DestroyRoomBlock(Registry.appState.UI.currentSelectedRoom, Registry.appState.UI.currentSelectedRoomBlock);
        }
    }
}
using System.Collections.Generic;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;

namespace TowerBuilder.Stores.UI
{
    public class DestroyToolState : ToolStateBase
    {
        public struct Input { }

        public DestroyToolState(UI.State state, Input input) : base(state) { }

        public void DestroyCurrentSelectedRoomBlock()
        {
            Registry.Stores.Rooms.DestroyRoomBlock(parentState.currentSelectedRoom, parentState.currentSelectedRoomBlock);
        }
    }
}
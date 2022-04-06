using System.Collections.Generic;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;

namespace TowerBuilder.Stores.UI
{
    public class DestroyToolState : ToolStateBase
    {
        public DestroyToolState(UI.State state) : base(state) { }

        public void DestroyCurrentSelectedRoomBlock()
        {
            Registry.Stores.Rooms.DestroyRoomBlock(parentState.currentSelectedRoom, parentState.currentSelectedRoomBlock);
        }
    }
}
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

        public void DestroyCurrentSelectedRoom()
        {
            DestroyRoom(parentState.currentSelectedRoom);
        }

        public void DestroyRoom(Room room)
        {
            Registry.Stores.Rooms.DestroyRoom(room);
        }
    }
}
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class DestroyToolState : ToolStateBase
    {
        public DestroyToolState(MapUI.State state) : base(state) { }

        public void DestroyCurrentSelectedRoom()
        {
            DestroyRoom(parentState.currentSelectedRoom);
        }

        public void DestroyRoom(Room room)
        {
            Registry.Stores.Map.DestroyRoom(room);
        }
    }
}
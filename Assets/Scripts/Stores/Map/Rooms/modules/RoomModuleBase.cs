using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Modules
{
    public abstract class RoomModuleBase
    {
        public abstract RoomModuleKey key { get; }
        public Room room { get; private set; }

        public RoomModuleBase(Room room)
        {
            this.room = room;
        }

        public abstract void Initialize();
        public abstract void OnDestroy();
    }
}
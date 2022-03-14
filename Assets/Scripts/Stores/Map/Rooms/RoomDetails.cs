using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Modules;
using TowerBuilder.Stores.Map.Rooms.Uses;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomDetails
    {
        public string title;
        public int price;
        public RoomCategory category = RoomCategory.None;

        public List<RoomUseDetailsBase> useDetails = new List<RoomUseDetailsBase>();
        public RoomPrivacy privacy = RoomPrivacy.Public;

        public int width = 1;
        public int height = 1;
        public RoomResizability resizability = RoomResizability.Inflexible();

        public List<RoomEntrance> entrances = new List<RoomEntrance>();

        public Color color = Color.white;
    }
}


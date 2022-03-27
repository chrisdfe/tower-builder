using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map.Rooms.Modules;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomDetails
    {
        public string title;
        public int price;
        public RoomCategory category = RoomCategory.None;

        public List<RoomModuleDetailsBase> moduleDetails = new List<RoomModuleDetailsBase>();
        public RoomPrivacy privacy = RoomPrivacy.Public;

        public int width = 1;
        public int height = 1;
        public RoomResizability resizability = RoomResizability.Inflexible();

        public List<RoomEntrance> entrances = new List<RoomEntrance>();

        public Color color = Color.white;
    }
}


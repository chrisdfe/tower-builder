using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomDetails
    {
        public string title;
        public int price;
        public RoomUse[] uses;

        public int width = 1;
        public int height = 1;
        public RoomResizability roomResizability = RoomResizability.Inflexible();

        public RoomPrivacy privacy;
        public List<RoomEntrance> entrances;

        public Color color = Color.white;
    }
}



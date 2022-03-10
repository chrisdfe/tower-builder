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
        public RoomUse[] uses = new RoomUse[0];

        public int width = 1;
        public int height = 1;
        public RoomResizability resizability = RoomResizability.Inflexible();

        public RoomPrivacy privacy = RoomPrivacy.Public;
        public List<RoomEntrance> entrances = new List<RoomEntrance>();

        public Color color = Color.white;
    }
}



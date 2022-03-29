using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map.Rooms.Modules;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomTemplate
    {
        public string title;

        public RoomPrivacy privacy = RoomPrivacy.Public;
        public List<RoomFurnitureConfigBase> furnitureConfigs = new List<RoomFurnitureConfigBase>();

        public Dimensions blockDimensions;
        public RoomResizability resizability = RoomResizability.Inflexible();

        public List<RoomEntrance> entrances = new List<RoomEntrance>();

        public Color color = Color.white;
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map.Rooms.EntranceBuilders;
using TowerBuilder.Stores.Map.Rooms.Furniture;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomDetails
    {
        public string title;
        public int price;
        public RoomCategory category = RoomCategory.None;

        public List<RoomFurnitureConfigBase> furnitureConfigs = new List<RoomFurnitureConfigBase>();
        public RoomPrivacy privacy = RoomPrivacy.Public;

        public int width = 1;
        public int height = 1;
        public RoomResizability resizability = RoomResizability.Inflexible();

        public RoomEntranceBuilderBase entranceBuilder;

        public Color color = Color.white;
    }
}


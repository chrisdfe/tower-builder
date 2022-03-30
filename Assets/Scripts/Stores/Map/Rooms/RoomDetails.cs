using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map.Rooms.Entrances;
using TowerBuilder.Stores.Map.Rooms.Furniture;
using TowerBuilder.Stores.Map.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomDetails
    {
        public string title;
        public int price;
        public RoomCategory category = RoomCategory.None;

        public List<RoomFurnitureAttributesBase> furnitureAttributes = new List<RoomFurnitureAttributesBase>();

        public int width = 1;
        public int height = 1;
        public RoomResizability resizability = RoomResizability.Inflexible();

        public RoomPrivacy privacy = RoomPrivacy.Public;
        public RoomEntranceBuilderBase entranceBuilder;

        public RoomValidatorBase validator = new DefaultRoomValidator();

        public Color color = Color.white;
    }
}


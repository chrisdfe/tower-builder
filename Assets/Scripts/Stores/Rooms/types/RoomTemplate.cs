using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Rooms.Entrances;
using TowerBuilder.Stores.Rooms.Furniture;
using TowerBuilder.Stores.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Stores.Rooms
{
    public class RoomTemplate
    {
        public string title;
        public string key;

        // TODO - rename to "pricePerTile"
        public int price;

        public string category = "None";

        public List<RoomFurnitureAttributesBase> furnitureAttributes = new List<RoomFurnitureAttributesBase>();

        public Dimensions blockDimensions;

        public RoomResizability resizability = RoomResizability.Inflexible();

        public RoomPrivacy privacy = RoomPrivacy.Public;

        public delegate RoomEntranceBuilderBase EntranceBuilderFactory();
        public EntranceBuilderFactory entranceBuilderFactory = () => new DefaultEntranceBuilder();

        public delegate RoomValidatorBase RoomValidatorFactory();
        public RoomValidatorFactory validatorFactory = () => new DefaultRoomValidator();

        public Color color = Color.white;
    }
}


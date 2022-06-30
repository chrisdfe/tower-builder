using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.State.Rooms.Entrances;
using TowerBuilder.State.Rooms.Furniture;
using TowerBuilder.State.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.State.Rooms
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

        // public RoomPrivacy privacy = RoomPrivacy.Public;

        public delegate RoomEntranceBuilderBase EntranceBuilderFactory();
        public EntranceBuilderFactory entranceBuilderFactory = () => new DefaultEntranceBuilder();

        public delegate RoomValidatorBase RoomValidatorFactory();
        public RoomValidatorFactory validatorFactory = () => new DefaultRoomValidator();

        public Color color = Color.white;
    }
}


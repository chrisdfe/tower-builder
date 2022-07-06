using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Furniture;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomTemplate
    {
        public string title;
        public string key;

        // TODO - rename to "pricePerTile"
        public int price;

        public string category = "None";

        public List<FurnitureAttributesBase> furnitureAttributes = new List<FurnitureAttributesBase>();

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


using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomTemplate
    {
        public string title;
        public string key;

        public int pricePerBlock;

        public string category = "None";

        public Dimensions blockDimensions;

        public RoomResizability resizability = RoomResizability.Inflexible;

        public delegate RoomEntranceBuilderBase EntranceBuilderFactory();
        public EntranceBuilderFactory entranceBuilderFactory = () => new DefaultEntranceBuilder();

        public delegate RoomFurnitureBuilder FurnitureBuilderFactory(Room room);
        public FurnitureBuilderFactory furnitureBuilderFactory = (Room room) => new RoomFurnitureBuilder(room);

        public delegate RoomValidatorBase RoomValidatorFactory(Room room);
        public RoomValidatorFactory validatorFactory = (Room room) => new DefaultRoomValidator(room);

        public RoomSkinKey skinKey = RoomSkinKey.Default;

        [NonSerialized]
        public Color color = Color.white;
    }
}


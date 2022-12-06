using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomTemplate
    {
        public string title = "None";
        public string key = "None";
        public string category = "None";

        public Dimensions blockDimensions;

        public Room.Resizability resizability = Room.Resizability.Inflexible;

        public delegate RoomFurnitureBuilderBase FurnitureBuilderFactory(Room room);
        public FurnitureBuilderFactory furnitureBuilderFactory = (Room room) => new RoomFurnitureBuilderBase(room);

        public delegate RoomValidatorBase RoomValidatorFactory(Room room);
        public RoomValidatorFactory validatorFactory = (Room room) => new DefaultRoomValidator(room);

        public Room.SkinKey skinKey = Room.SkinKey.Default;
    }
}


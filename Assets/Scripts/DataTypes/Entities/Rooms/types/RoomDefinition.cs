using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Entities.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms
{
    public class RoomDefinition : EntityDefinition<Room.Key>
    {
        public delegate RoomFurnitureBuilderBase FurnitureBuilderFactory(Room room);
        public FurnitureBuilderFactory furnitureBuilderFactory = (Room room) => new RoomFurnitureBuilderBase(room);

        public Room.Skin.Key skinKey = Room.Skin.Key.Default;

        public new ValidatorFactory validatorFactory = (Entity entity) => new RoomValidator(entity as Room);
    }
}


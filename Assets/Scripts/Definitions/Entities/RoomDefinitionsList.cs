using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Entities.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class RoomDefinitionsList : EntityDefinitionsList<Room.Key, Room, RoomDefinition>
    {
        public override List<RoomDefinition> Definitions { get; } = new List<RoomDefinition>() {
            new RoomDefinition()
            {
                title = "Empty",
                key = Room.Key.Default,
                category = "Empty",

                blockSize = new Dimensions(1, 1),
                resizability = Room.Resizability.Flexible,
                pricePerCell = 1000,
            },

            new RoomDefinition()
            {
                title = "Wheels",
                key = Room.Key.Wheels,
                category = "Wheels",

                blockSize = new Dimensions(1, 1),
                resizability = Room.Resizability.Horizontal,
                pricePerCell = 5000,

                skinKey = Room.Skin.Key.Wheels
            },
        };

        public RoomDefinitionsList() : base() { }
    }
}

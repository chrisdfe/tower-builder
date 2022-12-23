using System.Collections.Generic;
using Newtonsoft.Json;
using TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Entities.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms
{
    public partial class Room : Entity<Room.Key>
    {
        public enum Key
        {
            Default,
            OtherDefault,
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Default, "Default" },
                { Key.OtherDefault, "OtherDefault" }
            }
        );

        public enum SkinKey
        {
            Default
        }

        public override string idKey => "Rooms";

        public SkinKey skinKey;

        public Dimensions blockDimensions { get; } = Dimensions.one;

        public int cellsInBlock => blockDimensions.width * blockDimensions.height;

        // TODO - get rid of this
        public RoomFurnitureBuilderBase furnitureBuilder { get; }

        public Room(RoomDefinition roomDefinition) : base(roomDefinition)
        {
            // TODO - get rid of this
            this.furnitureBuilder = roomDefinition.furnitureBuilderFactory(this);

            this.skinKey = roomDefinition.skinKey;
        }
    }
}



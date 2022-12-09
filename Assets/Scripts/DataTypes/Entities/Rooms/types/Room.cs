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
            Wheels
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Default, "Default" },
                { Key.Wheels,  "Wheels" },
            }
        );

        public override string idKey => "Rooms";


        public Dimensions blockDimensions { get; } = Dimensions.one;

        public int cellsInBlock
        {
            get => blockDimensions.width * blockDimensions.height;
        }

        // TODO - get rid of this
        public RoomFurnitureBuilderBase furnitureBuilder { get; }

        public Skin skin;

        public Room(RoomDefinition roomDefinition) : base(roomDefinition)
        {
            // TODO - get rid of this
            this.furnitureBuilder = roomDefinition.furnitureBuilderFactory(this);

            this.skin = new Skin(roomDefinition.skinKey);
        }

        public override string ToString() => $"room {id}";
    }
}



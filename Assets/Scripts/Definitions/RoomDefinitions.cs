using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Entities.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class RoomDefinitions
    {
        public Queries queries;

        public List<RoomTemplate> templates { get; } = new List<RoomTemplate>() {
            new RoomTemplate()
            {
                title = "Empty",
                key = Room.Key.Default,
                category = "Empty",

                blockDimensions = new Dimensions(1, 1),
                resizability = Room.Resizability.Flexible,

                validatorFactory = (Room room) => new DefaultRoomValidator(room),
            },

            new RoomTemplate()
            {
                title = "Wheels",
                key = Room.Key.Wheels,
                category = "Wheels",

                blockDimensions = new Dimensions(1, 1),
                resizability = Room.Resizability.Horizontal,

                validatorFactory = (Room room) => new WheelsRoomValidator(room),

                skinKey = Room.Skin.Key.Wheels
            },
        };

        public class Queries
        {
            List<RoomTemplate> definitions;

            public Queries(List<RoomTemplate> definitions)
            {
                this.definitions = definitions;
            }

            public RoomTemplate FindByTitle(string title) =>
                definitions.Find(template => template.title == title);

            public RoomTemplate FindByKey(Room.Key key) =>
                definitions.Find(template => template.key == key);

            public List<RoomTemplate> FindByCategory(string category) =>
                definitions.FindAll(template => template.category == category).ToList();

            public List<string> FindAllCategories() =>
                definitions.Aggregate(new List<string>(), (acc, definition) =>
                {
                    if (!acc.Contains(definition.category))
                    {
                        acc.Add(definition.category);
                    }

                    return acc;
                });
        }

        public RoomDefinitions()
        {
            this.queries = new Queries(templates);
        }
    }
}

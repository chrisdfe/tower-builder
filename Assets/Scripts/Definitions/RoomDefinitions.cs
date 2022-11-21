using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Definitions
{
    public class RoomDefinitions
    {
        public Queries queries;

        public List<RoomTemplate> templates { get; } = new List<RoomTemplate>() {
            new RoomTemplate()
            {
                title = "Hallway",
                key = "Hallway",
                category = "Hallway",

                pricePerBlock = 500,

                blockDimensions = new Dimensions(1, 1),
                resizability = RoomResizability.Horizontal,

                entranceBuilderFactory = () => new HallwayEntranceBuilder(),
                validatorFactory = (Room room) => new DefaultRoomValidator(room),

                color = Color.gray
            },

            new RoomTemplate()
            {
                title = "Wheels",
                key = "Wheels",
                category = "Wheels",

                pricePerBlock = 5000,

                blockDimensions = new Dimensions(1, 1),
                resizability = RoomResizability.Horizontal,

                entranceBuilderFactory = () => new EmptyEntranceBuilder(),
                validatorFactory = (Room room) => new WheelsRoomValidator(room),

                color = Color.gray,

                skinKey = RoomSkinKey.Wheels
            },

            // new RoomTemplate()
            // {
            //     title = "Large Wheels",
            //     key = "LargeWheels",
            //     category = "Wheels",

            //     pricePerBlock = 5000,

            //     blockDimensions = new Dimensions(1, 2),
            //     resizability = RoomResizability.Horizontal,

            //     entranceBuilderFactory = () => new EmptyEntranceBuilder(),
            //     validatorFactory = (Room room) => new WheelsRoomValidator(room),

            //     color = Color.gray,
            // },

            new RoomTemplate()
            {
                title = "Office",
                key = "Office",

                pricePerBlock = 20000,
                category = "Office",

                blockDimensions = new Dimensions(3, 1),

                entranceBuilderFactory = () => new InflexibleRoomEntranceBuilder(new List<RoomEntrance>()
                    {
                        new RoomEntrance() {
                            position = RoomEntrancePosition.Left,
                            cellCoordinates = new CellCoordinates(0, 0)
                        },
                        new RoomEntrance() {
                            position = RoomEntrancePosition.Right,
                            cellCoordinates = new CellCoordinates(2, 0)
                        },
                    }
                ),

                validatorFactory = (Room room) => new DefaultRoomValidator(room),

                color = Color.green,
            },

            new RoomTemplate()
            {
                title = "Barracks",
                key = "Barracks",

                pricePerBlock = 30000,
                category = "Residence",

                blockDimensions = new Dimensions(2, 1),

                entranceBuilderFactory = () => new InflexibleRoomEntranceBuilder(new List<RoomEntrance>()
                    {
                        new RoomEntrance()
                        {
                            position = RoomEntrancePosition.Left,
                            cellCoordinates = new CellCoordinates(0, 0)
                        },
                        new RoomEntrance()
                        {
                            position = RoomEntrancePosition.Right,
                            cellCoordinates = new CellCoordinates(1, 0)
                        },
                    }
                ),

                validatorFactory = (Room room) => new DefaultRoomValidator(room),

                color = Color.yellow,
            },

            new RoomTemplate()
            {
                title = "Bedroom",
                key = "Bedroom",

                pricePerBlock = 12000,
                category = "Residence",

                blockDimensions = new Dimensions(2, 1),

                entranceBuilderFactory = () => new InflexibleRoomEntranceBuilder(new List<RoomEntrance>()
                    {
                        new RoomEntrance() {
                            position = RoomEntrancePosition.Left,
                            cellCoordinates = new CellCoordinates(0, 0)
                        },
                        new RoomEntrance() {
                            position = RoomEntrancePosition.Right,
                            cellCoordinates = new CellCoordinates(1, 0)
                        },
                    }
                ),

                validatorFactory = (Room room) => new DefaultRoomValidator(room),

                color = Color.yellow,
            },

            new RoomTemplate()
            {
                title = "Condo",
                key = "Condo",

                pricePerBlock = 50000,
                category = "Residence",

                blockDimensions = new Dimensions(5, 1),

                entranceBuilderFactory = () => new InflexibleRoomEntranceBuilder(new List<RoomEntrance>()
                    {
                        new RoomEntrance() {
                            position = RoomEntrancePosition.Left,
                            cellCoordinates = new CellCoordinates(0, 0)
                        },
                        new RoomEntrance() {
                            position = RoomEntrancePosition.Right,
                            cellCoordinates = new CellCoordinates(4, 0)
                        },
                    }
                ),

                validatorFactory = (Room room) => new DefaultRoomValidator(room),

                color = Color.yellow,
            },

            new RoomTemplate()
            {
                title = "Elevator",
                key = "Elevator",

                pricePerBlock = 2000,
                category = "Elevator",

                blockDimensions = new Dimensions(1, 1),

                resizability = RoomResizability.Vertical,

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),

                validatorFactory = (Room room) => new ElevatorRoomValidator(room),

                color = Color.magenta,
            },

            new RoomTemplate()
            {
                title = "Service Elevator",
                key = "ServiceElevator",

                pricePerBlock = 1500,
                category = "Elevator",

                blockDimensions = new Dimensions(1, 1),

                resizability = RoomResizability.Vertical,

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),

                validatorFactory = (Room room) => new ElevatorRoomValidator(room),

                color = Color.yellow,
            },

            new RoomTemplate()
            {
                title = "Large Elevator",
                key = "LargeElevator",

                pricePerBlock = 5000,
                category = "Elevator",

                blockDimensions = new Dimensions(2, 1),

                resizability = RoomResizability.Vertical,

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),
                validatorFactory = (Room room) => new ElevatorRoomValidator(room),

                color = Color.magenta,
            },

            new RoomTemplate()
            {
                title = "Stairwell",
                key = "Stairwell",

                category = "Stairs",

                pricePerBlock = 5000,

                blockDimensions = new Dimensions(1, 1),

                resizability = RoomResizability.Vertical,
                color = Color.yellow,

                entranceBuilderFactory = () => new StairwellEntranceBuilder(),
                validatorFactory = (Room room) => new DefaultRoomValidator(room),

            },

            new RoomTemplate()
            {
                title = "Large Stairwell",
                key = "LargeStairwell",

                pricePerBlock = 8000,

                category = "Stairs",

                blockDimensions = new Dimensions(2, 1),

                resizability = RoomResizability.Vertical,
                color = Color.yellow,

                entranceBuilderFactory = () => new StairwellEntranceBuilder(),
                validatorFactory = (Room room) => new DefaultRoomValidator(room),

            },

            new RoomTemplate()
            {
                title = "Small Park",
                key = "SmallPark",

                pricePerBlock = 10000,

                category = "Park",

                blockDimensions = new Dimensions(1, 1),

                resizability = RoomResizability.Flexible,

                entranceBuilderFactory = () => new InflexibleRoomEntranceBuilder(new List<RoomEntrance>()
                {
                    new RoomEntrance() {
                        position = RoomEntrancePosition.Left,
                        cellCoordinates = new CellCoordinates(0, 0)
                    },
                    new RoomEntrance() {
                        position = RoomEntrancePosition.Right,
                        cellCoordinates = new CellCoordinates(0, 0)
                    },
                }),
                validatorFactory = (Room room) => new DefaultRoomValidator(room),


                color = Color.green,
            }
        };

        public class Queries
        {
            List<RoomTemplate> definitions;

            public Queries(List<RoomTemplate> definitions)
            {
                this.definitions = definitions;
            }

            public RoomTemplate FindByTitle(string title)
            {
                return definitions.Find(template => template.title == title);
            }

            public RoomTemplate FindByKey(string key)
            {
                return definitions.Find(template => template.key == key);
            }

            public List<RoomTemplate> FindByCategory(string category)
            {
                return definitions.FindAll(template => template.category == category).ToList();
            }

            public List<string> FindAllCategories()
            {
                List<string> result = new List<string>();

                foreach (RoomTemplate roomTemplate in definitions)
                {
                    if (!result.Contains(roomTemplate.category))
                    {
                        result.Add(roomTemplate.category);
                    }
                }

                return result;
            }
        }

        public RoomDefinitions()
        {
            this.queries = new Queries(templates);
        }
    }
}

using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Templates
{
    public static class DefaultRoomTemplates
    {
        public static List<RoomTemplate> roomTemplates = new List<RoomTemplate>()
        {
            new RoomTemplate()
            {
                title = "Hallway",
                key = "Hallway",
                category = "Hallway",

                pricePerBlock = 500,

                blockDimensions = new Dimensions(1, 1),
                resizability = RoomResizability.Flexible(),

                entranceBuilderFactory = () => new HallwayEntranceBuilder(),
                validatorFactory = () => new DefaultRoomValidator(),

                color = Color.gray
            },

            new RoomTemplate()
            {
                title = "Lobby",
                key = "Lobby",
                category = "Lobby",

                pricePerBlock = 5000,

                blockDimensions = new Dimensions(1, 1),
                resizability = RoomResizability.Horizontal(),

                entranceBuilderFactory = () => new LobbyEntranceBuilder(),
                validatorFactory = () => new LobbyRoomValidator(),

                color = Color.gray,
            },

            new RoomTemplate()
            {
                title = "Wheels",
                key = "Wheels",
                category = "Wheels",

                pricePerBlock = 5000,

                blockDimensions = new Dimensions(1, 1),
                resizability = RoomResizability.Horizontal(),

                entranceBuilderFactory = () => new EmptyEntranceBuilder(),
                validatorFactory = () => new WheelsRoomValidator(),

                color = Color.gray,
            },

            new RoomTemplate()
            {
                title = "Large Lobby",
                key = "LargeLobby",

                pricePerBlock = 12000,
                category = "Lobby",

                blockDimensions = new Dimensions(1, 2),

                resizability = RoomResizability.Horizontal(),

                entranceBuilderFactory = () => new LobbyEntranceBuilder(),
                validatorFactory = () => new LobbyRoomValidator(),

                color = Color.gray,
            },

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

                validatorFactory = () => new DefaultRoomValidator(),

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

                validatorFactory = () => new DefaultRoomValidator(),

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

                validatorFactory = () => new DefaultRoomValidator(),

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

                validatorFactory = () => new DefaultRoomValidator(),

                color = Color.yellow,
            },

            new RoomTemplate()
            {
                title = "Elevator",
                key = "Elevator",

                pricePerBlock = 2000,
                category = "Elevator",

                blockDimensions = new Dimensions(1, 1),

                resizability = RoomResizability.Vertical(),

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),

                validatorFactory = () => new ElevatorRoomValidator(),

                color = Color.magenta,
            },

            new RoomTemplate()
            {
                title = "Service Elevator",
                key = "ServiceElevator",

                pricePerBlock = 1500,
                category = "Elevator",

                blockDimensions = new Dimensions(1, 1),

                resizability = RoomResizability.Vertical(),

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),

                validatorFactory = () => new ElevatorRoomValidator(),

                color = Color.yellow,
            },

            new RoomTemplate()
            {
                title = "Large Elevator",
                key = "LargeElevator",

                pricePerBlock = 5000,
                category = "Elevator",

                blockDimensions = new Dimensions(2, 1),

                resizability = RoomResizability.Vertical(),

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),
                validatorFactory = () => new ElevatorRoomValidator(),

                color = Color.magenta,
            },

            new RoomTemplate()
            {
                title = "Stairwell",
                key = "Stairwell",

                category = "Stairs",

                pricePerBlock = 5000,

                blockDimensions = new Dimensions(1, 1),

                resizability = RoomResizability.Vertical(),
                color = Color.yellow,

                entranceBuilderFactory = () => new StairwellEntranceBuilder(),
                validatorFactory = () => new DefaultRoomValidator(),

            },

            new RoomTemplate()
            {
                title = "Large Stairwell",
                key = "LargeStairwell",

                pricePerBlock = 8000,

                category = "Stairs",

                blockDimensions = new Dimensions(2, 1),

                resizability = RoomResizability.Vertical(),
                color = Color.yellow,

                entranceBuilderFactory = () => new StairwellEntranceBuilder(),
                validatorFactory = () => new DefaultRoomValidator(),

            },

            new RoomTemplate()
            {
                title = "Small Park",
                key = "SmallPark",

                pricePerBlock = 10000,

                category = "Park",

                blockDimensions = new Dimensions(1, 1),

                resizability = RoomResizability.Flexible(),

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
                validatorFactory = () => new DefaultRoomValidator(),


                color = Color.green,
            }
        };
    }
}
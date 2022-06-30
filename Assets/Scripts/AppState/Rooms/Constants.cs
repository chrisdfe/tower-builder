using System;
using System.Collections;
using System.Collections.Generic;


using TowerBuilder.State.Rooms.Entrances;
using TowerBuilder.State.Rooms.Validators;

using UnityEngine;

namespace TowerBuilder.State.Rooms
{
    public static class Constants
    {
        // TODO - this should go somewhere else
        public static float TILE_SIZE = 1f;

        public static List<RoomTemplate> ROOM_DEFINITIONS = new List<RoomTemplate>()
        {
            new RoomTemplate()
            {
                title = "Hallway",
                key = "Hallway",
                category = "Hallway",

                price = 500,

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

                price = 5000,

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

                price = 5000,

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

                price = 12000,
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

                price = 20000,
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

                price = 30000,
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

                price = 12000,
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

                price = 50000,
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

                price = 2000,
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

                price = 1500,
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

                price = 5000,
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

                price = 5000,

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

                price = 8000,

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

                price = 10000,

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

        static Constants()
        {

        }
    }
}
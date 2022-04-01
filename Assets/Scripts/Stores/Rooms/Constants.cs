using System;
using System.Collections;
using System.Collections.Generic;


using TowerBuilder.Stores.Rooms.Entrances;
using TowerBuilder.Stores.Rooms.Validators;

using UnityEngine;

namespace TowerBuilder.Stores.Rooms
{
    public static class Constants
    {
        // TODO - this should go somewhere else
        public static float TILE_SIZE = 1f;

        public static Dictionary<RoomKey, RoomDetails> ROOM_DETAILS_MAP = new Dictionary<RoomKey, RoomDetails>()
        {
            [RoomKey.None] = new RoomDetails(),

            [RoomKey.Hallway] = new RoomDetails()
            {
                title = "Hallway",
                price = 500,
                category = RoomCategory.Hallway,
                width = 1,
                height = 1,
                resizability = RoomResizability.Flexible(),
                privacy = RoomPrivacy.Public,

                entranceBuilderFactory = () => new HallwayEntranceBuilder(),
                validatorFactory = () => new DefaultRoomValidator(),

                color = Color.gray
            },

            [RoomKey.Lobby] = new RoomDetails()
            {
                title = "Lobby",
                price = 5000,
                category = RoomCategory.Lobby,
                width = 1,
                height = 1,
                resizability = RoomResizability.Horizontal(),
                privacy = RoomPrivacy.Public,

                entranceBuilderFactory = () => new LobbyEntranceBuilder(),
                validatorFactory = () => new LobbyRoomValidator(),

                color = Color.red,
            },

            [RoomKey.LargeLobby] = new RoomDetails()
            {
                title = "Large Lobby",
                price = 12000,
                category = RoomCategory.Lobby,
                width = 1,
                height = 2,
                resizability = RoomResizability.Horizontal(),
                privacy = RoomPrivacy.Public,

                entranceBuilderFactory = () => new LobbyEntranceBuilder(),
                validatorFactory = () => new LobbyRoomValidator(),

                color = Color.red,
            },

            [RoomKey.Office] = new RoomDetails()
            {
                title = "Office",
                price = 20000,
                category = RoomCategory.Office,
                width = 3,
                height = 1,
                privacy = RoomPrivacy.Private,
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

            [RoomKey.Barracks] = new RoomDetails()
            {
                title = "Barracks",
                price = 30000,
                category = RoomCategory.Residence,

                width = 2,
                height = 1,
                privacy = RoomPrivacy.Private,

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

            [RoomKey.Bedroom] = new RoomDetails()
            {
                title = "Bedroom",
                price = 12000,
                category = RoomCategory.Residence,

                width = 2,
                height = 1,
                privacy = RoomPrivacy.Private,

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

            [RoomKey.Condo] = new RoomDetails()
            {
                title = "Condo",
                price = 50000,
                category = RoomCategory.Residence,
                width = 5,
                height = 1,
                privacy = RoomPrivacy.Private,

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

            [RoomKey.Elevator] = new RoomDetails()
            {
                title = "Elevator",
                price = 2000,
                category = RoomCategory.Elevator,

                width = 1,
                height = 1,

                resizability = RoomResizability.Vertical(),
                privacy = RoomPrivacy.Public,

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),

                validatorFactory = () => new ElevatorRoomValidator(),

                color = Color.magenta,
            },

            [RoomKey.ServiceElevator] = new RoomDetails()
            {
                title = "Service Elevator",
                price = 1500,
                category = RoomCategory.Elevator,

                width = 1,
                height = 1,
                resizability = RoomResizability.Vertical(),
                privacy = RoomPrivacy.Private,

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),

                validatorFactory = () => new ElevatorRoomValidator(),

                color = Color.yellow,
            },

            [RoomKey.LargeElevator] = new RoomDetails()
            {
                title = "Large Elevator",
                price = 5000,
                category = RoomCategory.Elevator,

                width = 2,
                height = 1,
                resizability = RoomResizability.Vertical(),
                privacy = RoomPrivacy.Public,

                entranceBuilderFactory = () => new ElevatorEntranceBuilder(),
                validatorFactory = () => new ElevatorRoomValidator(),

                color = Color.magenta,
            },

            [RoomKey.Stairwell] = new RoomDetails()
            {
                title = "Stairwell",
                price = 5000,
                category = RoomCategory.Stairs,
                width = 1,
                height = 1,
                resizability = RoomResizability.Vertical(),
                color = Color.yellow,

                entranceBuilderFactory = () => new StairwellEntranceBuilder(),
                validatorFactory = () => new DefaultRoomValidator(),

                privacy = RoomPrivacy.Public,
            },

            [RoomKey.LargeStairwell] = new RoomDetails()
            {
                title = "Large Stairwell",
                price = 8000,
                category = RoomCategory.Stairs,
                width = 2,
                height = 1,
                resizability = RoomResizability.Vertical(),
                color = Color.yellow,

                entranceBuilderFactory = () => new StairwellEntranceBuilder(),
                validatorFactory = () => new DefaultRoomValidator(),

                privacy = RoomPrivacy.Public,
            },

            [RoomKey.SmallPark] = new RoomDetails()
            {
                title = "Small Park",
                price = 10000,
                category = RoomCategory.Park,
                width = 1,
                height = 1,
                color = Color.green,

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

                privacy = RoomPrivacy.Public,
            }
        };
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Modules;
using TowerBuilder.Stores.Map.Rooms.Uses;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public static class Constants
    {
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
                entrances = new List<RoomEntrance>() {
                    new RoomEntrance() {
                        position = RoomEntrancePosition.Left,
                        cellCoordinates = new CellCoordinates(0, 0)
                    },
                    new RoomEntrance() {
                        position = RoomEntrancePosition.Right,
                        cellCoordinates = new CellCoordinates(2, 0)
                    },
                },
                color = Color.green,
            },

            [RoomKey.Bedroom] = new RoomDetails()
            {
                title = "Bedroom",
                price = 12000,
                category = RoomCategory.Residence,
                useDetails = new List<RoomUseDetailsBase>()
                {
                    new Uses.ResidenceDetails()
                    {
                        occupancy = 2
                    }
                },
                width = 2,
                height = 1,
                privacy = RoomPrivacy.Private,
                entrances = new List<RoomEntrance>() {
                    new RoomEntrance() {
                        position = RoomEntrancePosition.Left,
                        cellCoordinates = new CellCoordinates(0, 0)
                    },
                    new RoomEntrance() {
                        position = RoomEntrancePosition.Right,
                        cellCoordinates = new CellCoordinates(1, 0)
                    },
                },
                color = Color.yellow,
            },

            [RoomKey.Condo] = new RoomDetails()
            {
                title = "Condo",
                price = 50000,
                category = RoomCategory.Residence,
                useDetails = new List<RoomUseDetailsBase>()
                {
                    new Uses.ResidenceDetails()
                    {
                        occupancy = 5
                    }
                },
                width = 5,
                height = 1,
                privacy = RoomPrivacy.Private,
                entrances = new List<RoomEntrance>() {
                    new RoomEntrance() {
                        position = RoomEntrancePosition.Left,
                        cellCoordinates = new CellCoordinates(0, 0)
                    },
                    new RoomEntrance() {
                        position = RoomEntrancePosition.Right,
                        cellCoordinates = new CellCoordinates(4, 0)
                    },
                },
                color = Color.yellow,
            },

            [RoomKey.Elevator] = new RoomDetails()
            {
                title = "Elevator",
                price = 2000,
                category = RoomCategory.Elevator,
                useDetails = new List<RoomUseDetailsBase>()
                {
                    new Uses.ElevatorDetails()
                    {
                        capacity = 5
                    }
                },
                width = 1,
                height = 1,
                resizability = RoomResizability.Vertical(),
                privacy = RoomPrivacy.Public,
                color = Color.magenta,
            },

            [RoomKey.Elevator] = new RoomDetails()
            {
                title = "Service Elevator",
                price = 1500,
                category = RoomCategory.Elevator,
                useDetails = new List<RoomUseDetailsBase>()
                {
                    new Uses.ElevatorDetails()
                    {
                        capacity = 10
                    }
                },
                width = 1,
                height = 1,
                resizability = RoomResizability.Vertical(),
                privacy = RoomPrivacy.Private,
                color = Color.yellow,
            },

            [RoomKey.LargeElevator] = new RoomDetails()
            {
                title = "Large Elevator",
                price = 5000,
                category = RoomCategory.Elevator,
                useDetails = new List<RoomUseDetailsBase>()
                {
                    new Uses.ElevatorDetails()
                    {
                        capacity = 20
                    }
                },
                width = 2,
                height = 1,
                resizability = RoomResizability.Vertical(),
                privacy = RoomPrivacy.Public,
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
                privacy = RoomPrivacy.Public,
            }
        };
    }
}
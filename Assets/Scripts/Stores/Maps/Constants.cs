using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public static class Constants
    {
        public static float TILE_SIZE = 1f;

        public static Dictionary<RoomKey, MapRoomDetails> ROOM_DETAILS_MAP = new Dictionary<RoomKey, MapRoomDetails>()
        {
            [RoomKey.None] = new MapRoomDetails(),
            [RoomKey.EmptyFloor] = new MapRoomDetails()
            {
                title = "Empty Floor",
                price = 500,
                uses = new RoomUse[] {
                    RoomUse.CommonArea,
                    RoomUse.Transportation,
                },
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomResizability = RoomResizability.Flexible(),
                color = Color.gray
            },

            [RoomKey.Lobby] = new MapRoomDetails()
            {
                title = "Lobby",
                price = 5000,
                uses = new RoomUse[] {
                    RoomUse.CommonArea,
                    RoomUse.Transportation
                },
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomResizability = RoomResizability.Horizontal(),
                color = Color.red,
            },

            [RoomKey.Office] = new MapRoomDetails()
            {
                title = "Office",
                price = 20000,
                uses = new RoomUse[] {
                    RoomUse.Workplace
                },
                roomCells = RoomCells.CreateRectangularRoom(3, 2),
                color = Color.green,
            },

            [RoomKey.Condo] = new MapRoomDetails()
            {
                title = "Condo",
                price = 50000,
                uses = new RoomUse[] {
                    RoomUse.Residence
                },
                roomCells = RoomCells.CreateRectangularRoom(5, 1),
                color = Color.yellow,
            },

            [RoomKey.Elevator] = new MapRoomDetails()
            {
                title = "Elevator",
                price = 2000,
                uses = new RoomUse[] {
                    RoomUse.Transportation
                },
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomResizability = RoomResizability.Vertical(),
                color = Color.magenta,
            },

            [RoomKey.Stairwell] = new MapRoomDetails()
            {
                title = "Stairwell",
                price = 5000,
                uses = new RoomUse[] {
                    RoomUse.Transportation
                },
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomResizability = RoomResizability.Vertical(),
                color = Color.white
            },

            [RoomKey.SmallPark] = new MapRoomDetails()
            {
                title = "Small Park",
                price = 10000,
                uses = new RoomUse[] {
                    RoomUse.CommonArea,
                    RoomUse.Park
                },
                roomCells = RoomCells.CreateRectangularRoom(2, 2),
                color = Color.green
            }
        };
    }
}
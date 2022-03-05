using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public static class Constants
    {
        public static float TILE_SIZE = 1f;

        public static Dictionary<RoomKey, MapRoomDetails> MAP_ROOM_DETAILS = new Dictionary<RoomKey, MapRoomDetails>()
        {
            [RoomKey.None] = new MapRoomDetails(),
            [RoomKey.EmptyFloor] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomResizability = RoomResizability.Flexible(),
                color = Color.gray
            },

            [RoomKey.Lobby] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomResizability = RoomResizability.Horizontal(),
                color = Color.red,
            },

            [RoomKey.Office] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(3, 2),
                color = Color.green,
            },

            [RoomKey.Condo] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(5, 1),
                color = Color.yellow,
            },

            [RoomKey.Elevator] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomResizability = RoomResizability.Vertical(),
                color = Color.magenta,
            },

            [RoomKey.Stairwell] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomResizability = RoomResizability.Vertical(),
                color = Color.white
            },

            [RoomKey.SmallPark] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(2, 2),
                color = Color.green
            }
        };
    }
}
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
            [RoomKey.EmptyFloor] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                color = Color.gray
            },
            [RoomKey.Lobby] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                roomBuildType = MapRoomBuildType.Flexible,
                color = Color.red,
            },
            [RoomKey.Office] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(3, 2),
                color = Color.green,
            },
            [RoomKey.Condo] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(5, 2),
                color = Color.yellow,
            },
            [RoomKey.Elevator] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
                color = Color.magenta,
            },
            [RoomKey.Stairwell] = new MapRoomDetails()
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1),
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
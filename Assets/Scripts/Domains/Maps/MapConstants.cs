using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Domains.Rooms;

namespace TowerBuilder.Domains.Map
{
    public static class MapConstants
    {
        public const int MAP_MAX_WIDTH = 20;
        public const int MAP_MAX_HEIGHT = 50;

        public const int MAP_VIEWPORT_WIDTH = 20;
        public const int MAP_VIEWPORT_HEIGHT = 20;

        public static RoomShapeMap ROOM_SHAPE_MAP = new RoomShapeMap()
        {
            // [RoomKey.EmptyFloor]: createRectangularRoomShape(1, 1),
            // [RoomKey.Lobby]: createRectangularRoomShape(1, 1),
            // [RoomKey.Office]: createRectangularRoomShape(2, 1),
            // [RoomKey.Condo]: createRectangularRoomShape(3, 1),
            // [RoomKey.Elevator]: createRectangularRoomShape(1, 1),
            // [RoomKey.Stairwell]: createRectangularRoomShape(2, 1),
            // [RoomKey.SmallPark]: createRectangularRoomShape(1, 1),
        };

        // Rooms that form groups when placed next to each other.
        public static RoomKey[] GROUPABLE_ROOM_KEYS = new RoomKey[] {
          RoomKey.Lobby,
          RoomKey.Elevator,
          RoomKey.Stairwell
        };

        public static RoomKey[] RESIDENTIAL_ROOM_KEYS = new RoomKey[] {
          RoomKey.Condo
        };
    }
}
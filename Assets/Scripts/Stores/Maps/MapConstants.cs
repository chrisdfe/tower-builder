using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static class Constants
        {
            public static float TILE_SIZE = 1f;

            // public static RoomShapeMap ROOM_SHAPE_MAP = new RoomShapeMap()
            // {
            //     // [RoomKey.EmptyFloor] = MapStore.Helpers.createRectangularRoomShape(1, 1),
            //     // [RoomKey.Lobby] = MapStore.Helpers.createRectangularRoomShape(1, 1),
            //     // [RoomKey.Office] = MapStore.Helpers.createRectangularRoomShape(2, 1),
            //     // [RoomKey.Condo] = MapStore.Helpers.createRectangularRoomShape(3, 1),
            //     // [RoomKey.Elevator] = MapStore.Helpers.createRectangularRoomShape(1, 1),
            //     // [RoomKey.Stairwell] = MapStore.Helpers.createRectangularRoomShape(2, 1),
            //     // [RoomKey.SmallPark] = MapStore.Helpers.createRectangularRoomShape(1, 1),
            // };

            // Rooms that form groups when placed next to each other.
            // public static RoomKey[] GROUPABLE_ROOM_KEYS = new RoomKey[] {
            //   RoomKey.Lobby,
            //   RoomKey.Elevator,
            //   RoomKey.Stairwell
            // };

            // public static RoomKey[] RESIDENTIAL_ROOM_KEYS = new RoomKey[] {
            //   RoomKey.Condo
            // };

            public static Dictionary<MapRoomRotation, CellCoordinates2D> MAP_ROOM_ROTATION_VALUES = new Dictionary<MapRoomRotation, CellCoordinates2D>()
            {
                [MapRoomRotation.Right] = new CellCoordinates2D(1, 1),
                [MapRoomRotation.Down] = new CellCoordinates2D(1, -1),
                [MapRoomRotation.Left] = new CellCoordinates2D(-1, -1),
                [MapRoomRotation.Up] = new CellCoordinates2D(-1, 1),
            };

            public static List<MapRoomRotation> ROOM_ROTATION_ORDER = new List<MapRoomRotation>() {
                MapRoomRotation.Right,
                MapRoomRotation.Down,
                MapRoomRotation.Left,
                MapRoomRotation.Up
            };
        }
    }
}
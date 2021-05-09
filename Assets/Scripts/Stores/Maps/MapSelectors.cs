using System;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static class Selectors
        {
            public static List<MapCoordinates> findRoomCellsById(StoreRegistry storeRegistry, string roomId)
            {
                RoomCellsMap roomCellsMap = storeRegistry.mapStore.state.roomCellsMap;
                return MapStore.Helpers.findRoomCellsById(roomId, roomCellsMap);
            }

            public static string findRoomIdAtCell(
                StoreRegistry storeRegistry,
                MapCoordinates coordinates
            )
            {
                RoomCellsMap roomCellsMap = storeRegistry.mapStore.state.roomCellsMap;
                return MapStore.Helpers.findRoomIdAtCell(coordinates, roomCellsMap);
            }

            public static List<string> findRoomGroupById(StoreRegistry storeRegistry, string roomGroupId)
            {
                RoomGroupMap roomGroupMap = storeRegistry.mapStore.state.roomGroupMap;
                return MapStore.Helpers.findRoomGroupById(roomGroupId, roomGroupMap);
            }
        }
    }
}
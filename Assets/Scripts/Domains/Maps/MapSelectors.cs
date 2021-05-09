using System;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Map
{
    public static class MapSelectors
    {
        public static List<MapCoordinates> findRoomCellsById(StoreRegistry storeRegistry, string roomId)
        {
            RoomCellsMap roomCellsMap = storeRegistry.mapStore.state.roomCellsMap;
            return RoomCellsMapHelpers.findRoomCellsById(roomId, roomCellsMap);
        }

        public static string findRoomIdAtCell(
            StoreRegistry storeRegistry,
            MapCoordinates coordinates
        )
        {
            RoomCellsMap roomCellsMap = storeRegistry.mapStore.state.roomCellsMap;
            return RoomCellsMapHelpers.findRoomIdAtCell(coordinates, roomCellsMap);
        }

        public static List<string> findRoomGroupById(StoreRegistry storeRegistry, string roomGroupId)
        {
            RoomGroupMap roomGroupMap = storeRegistry.mapStore.state.roomGroupMap;
            return RoomGroupMapHelpers.findRoomGroupById(roomGroupId, roomGroupMap);
        }
    }
}
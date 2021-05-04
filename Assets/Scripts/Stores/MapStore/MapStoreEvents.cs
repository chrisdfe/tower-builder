namespace TowerBuilder.Stores
{
    public static class MapStoreEvents
    {
        public struct RoomCellsEventPayload
        {
            string roomId;
            MapCoordinates[] roomCellsList;
        };

        public delegate void OnRoomCellsAdded(RoomCellsEventPayload payload);
        public delegate void OnRoomCellsRemoved(RoomCellsEventPayload payload);

        public struct RoomGroupEventPayload
        {
            string roomGroupId;
            RoomGroup roomGroup;
        };

        public delegate void OnRoomGroupAdded(RoomGroupEventPayload payload);
        public delegate void OnRoomGroupUpdated(RoomGroupEventPayload payload);
        public delegate void OnRoomGroupRemoved(RoomGroupEventPayload payload);

        public struct RoomGroupRoomEventPayload
        {
            string roomGroupId;
            RoomGroup roomGroup;
            string roomId;
        };

        public delegate void OnRoomAddedToRoomGroup(RoomGroupRoomEventPayload payload);
        public delegate void OnRoomRemovedFromRoomGroup(RoomGroupRoomEventPayload payload);
    }
}
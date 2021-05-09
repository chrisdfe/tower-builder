namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static class Events
        {
            public struct RoomCellsEventPayload
            {
                string roomId;
                MapCoordinates[] roomCellsList;
            };

            public delegate void RoomCellsEvent(RoomCellsEventPayload payload);

            public static event RoomCellsEvent onRoomCellsAdded;
            public static event RoomCellsEvent onRoomCellsRemoved;

            public struct RoomGroupEventPayload
            {
                string roomGroupId;
                RoomGroup roomGroup;
            };

            public delegate void RoomGroupEvent(RoomGroupEventPayload payload);

            public static event RoomGroupEvent onRoomGroupAdded;
            public static event RoomGroupEvent onRoomGroupUpdated;
            public static event RoomGroupEvent onRoomGroupRemoved;

            public struct RoomGroupRoomEventPayload
            {
                string roomGroupId;
                RoomGroup roomGroup;
                string roomId;
            };

            public delegate void RoomGroupRoomEvent(RoomGroupEventPayload payload);

            public static event RoomGroupRoomEvent onRoomAddedToRoomGroup;
            public static event RoomGroupRoomEvent OnRoomRemovedFromRoomGroup;

            public struct MapStateEventPayload
            {
                MapState state;
                MapState previousState;
            };

            public delegate void OnMapStateUpdated(MapStateEventPayload payload);
            public static event OnMapStateUpdated onMapStateUpdated;
        }
    }
}
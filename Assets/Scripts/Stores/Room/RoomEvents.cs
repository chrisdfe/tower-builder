namespace TowerBuilder.Stores.Rooms
{
    public partial class RoomStore
    {
        public static class Events
        {
            public struct RoomEventPayload
            {
                string roomId;
                RoomKey roomKey;
            };

            public delegate void RoomEvent(RoomEventPayload payload);

            public static event RoomEvent onRoomCreated;
            public static event RoomEvent onRoomDestroyed;

            public struct RoomStateEventPayload
            {
                RoomState state;
                RoomState previousState;
            };

            public delegate void OnRoomStateUpdated(RoomStateEventPayload payload);
            public static event OnRoomStateUpdated onRoomStateUpdated;
        }
    }
}
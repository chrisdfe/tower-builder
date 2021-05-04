namespace TowerBuilder.Stores
{
    public struct RoomEventPayload
    {
        string roomId;
        RoomKey roomKey;
    };

    public delegate void OnRoomStateUpdated(RoomState roomState, RoomState previousState);
    public delegate void OnRoomCreated(RoomEventPayload payload);
    public delegate void OnRoomDestroyed(RoomEventPayload payload);
}
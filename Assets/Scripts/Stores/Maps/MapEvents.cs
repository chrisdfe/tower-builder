namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static class Events
        {
            // public struct RoomCellsEventPayload
            // {
            //     string roomId;
            //     MapCoordinates[] roomCellsList;
            // };

            // public delegate void RoomCellsEvent(RoomCellsEventPayload payload);

            // public static event RoomCellsEvent onRoomCellsAdded;
            // public static event RoomCellsEvent onRoomCellsRemoved;


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
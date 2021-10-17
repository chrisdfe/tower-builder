using System;

namespace TowerBuilder.Stores.MapUI
{
    public partial class MapUIStore
    {
        public struct StateEventPayload
        {
            public MapUIState state;
            public MapUIState previousState;
        }

        public static class Events
        {
            public delegate void OnMapUIStateUpdated(StateEventPayload payload);
            public static OnMapUIStateUpdated onMapUIStateUpdated;
        }
    }
}

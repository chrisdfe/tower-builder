using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static class Events
        {
            static Events()
            {
                onMapStateUpdated += StateChangeSelectors;
            }

            public struct StateEventPayload
            {
                public MapState previousState;
                public MapState state;
            };

            public delegate void OnMapStateUpdated(StateEventPayload payload);
            public static OnMapStateUpdated onMapStateUpdated;

            public delegate void MapRoomEvent(MapRoom mapRoom);
            public static MapRoomEvent onMapRoomAdded;

            public delegate void MapRoomsEvent(List<MapRoom> mapRooms);
            public static MapRoomEvent onMapRoomsAdded;

            // TODO - rename this
            public static void StateChangeSelectors(StateEventPayload payload)
            {
                MapState previousState = payload.previousState;
                MapState state = payload.state;

            }
        }
    }
}
using System;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public partial class MapStore
    {
        public static class Mutations
        {
            // TODO - maybe all mutations should be like this instead
            public static void AddRoom(MapRoom newRoom)
            {
                Registry.Stores.mapStore.state.mapRooms.Add(newRoom);
                MapStore.Events.onMapRoomAdded(newRoom);
            }

            public static void FireUpdateEvent(MapState previousState, MapState state)
            {
                if (MapStore.Events.onMapStateUpdated != null)
                {
                    MapStore.Events.onMapStateUpdated(new MapStore.Events.StateEventPayload()
                    {
                        previousState = previousState,
                        state = state
                    });
                }
            }
        }
    }
}
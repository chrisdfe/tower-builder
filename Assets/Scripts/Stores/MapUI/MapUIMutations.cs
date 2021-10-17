using System;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public partial class MapUIStore
    {
        public static class Mutations
        {
            public static void SetToolState(ToolState newToolState)
            {
                MapUIStore gameUIStore = Registry.storeRegistry.mapUIStore;
                MapUIState previousState = gameUIStore.state;
                gameUIStore.state.toolState = newToolState;

                MapUIState state = gameUIStore.state;

                FireUpdateEvent(previousState, state);
            }

            public static void SetSelectedRoomKey(RoomKey selectedRoomKey)
            {
                MapUIStore gameUIStore = Registry.storeRegistry.mapUIStore;
                MapUIState previousState = gameUIStore.state;
                gameUIStore.state.selectedRoomKey = selectedRoomKey;

                MapUIState state = gameUIStore.state;
                FireUpdateEvent(previousState, state);
            }

            public static void SetCurrentFocusFloor(int focusFloor)
            {
                MapUIStore gameUIStore = Registry.storeRegistry.mapUIStore;
                MapUIState previousState = gameUIStore.state;

                gameUIStore.state.currentFocusFloor = focusFloor;

                MapUIState state = gameUIStore.state;
                FireUpdateEvent(previousState, state);
            }

            public static void FireUpdateEvent(MapUIState previousState, MapUIState state)
            {

                if (MapUIStore.Events.onMapUIStateUpdated != null)
                {
                    MapUIStore.Events.onMapUIStateUpdated(new MapUIStore.StateEventPayload()
                    {
                        previousState = previousState,
                        state = state
                    });
                }
            }
        }
    }
}
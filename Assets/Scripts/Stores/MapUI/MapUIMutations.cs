using System;
using TowerBuilder.Stores.Map;
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
                MapUIStore mapUIStore = Registry.Stores.mapUIStore;
                MapUIState previousState = mapUIStore.state;

                mapUIStore.state.toolState = newToolState;

                MapUIState state = mapUIStore.state;

                FireUpdateEvent(previousState, state);
            }

            public static void SetSelectedRoomKey(RoomKey selectedRoomKey)
            {
                MapUIStore mapUIStore = Registry.Stores.mapUIStore;
                MapUIState previousState = mapUIStore.state;

                mapUIStore.state.selectedRoomKey = selectedRoomKey;

                MapUIState state = mapUIStore.state;
                FireUpdateEvent(previousState, state);
            }

            public static void SetCurrentFocusFloor(int focusFloor)
            {
                MapUIStore mapUIStore = Registry.Stores.mapUIStore;
                MapUIState previousState = mapUIStore.state;

                mapUIStore.state.currentFocusFloor = focusFloor;

                MapUIState state = mapUIStore.state;
                FireUpdateEvent(previousState, state);
            }

            public static void SetCurrentSelectedCell(CellCoordinates selectedCell)
            {
                MapUIStore mapUIStore = Registry.Stores.mapUIStore;
                MapUIState previousState = mapUIStore.state;

                mapUIStore.state.currentSelectedTile = selectedCell;

                MapUIState state = mapUIStore.state;
                FireUpdateEvent(previousState, state);
            }

            public static void SetCurrentBlueprintRotation(MapRoomRotation rotation)
            {
                MapRoomRotation previousRotation = Registry.Stores.mapUIStore.state.currentBlueprintRotation;
                Registry.Stores.mapUIStore.state.currentBlueprintRotation = rotation;

                if (MapUIStore.Events.onBlueprintRotationUpdated != null)
                {
                    MapUIStore.Events.onBlueprintRotationUpdated(rotation, previousRotation);
                }
            }

            public static void FireUpdateEvent(MapUIState previousState, MapUIState state)
            {
                if (MapUIStore.Events.onMapUIStateUpdated != null)
                {
                    MapUIStore.Events.onMapUIStateUpdated(new MapUIStore.Events.StateEventPayload()
                    {
                        previousState = previousState,
                        state = state
                    });
                }
            }
        }
    }
}
using System;

using TowerBuilder.Stores.Map;

namespace TowerBuilder.Stores.MapUI
{
    public partial class MapUIStore
    {

        public static class Events
        {
            public struct StateEventPayload
            {
                public MapUIState state;
                public MapUIState previousState;
            }

            public delegate void OnMapUIStateUpdated(StateEventPayload payload);
            public static OnMapUIStateUpdated onMapUIStateUpdated;

            public static Events.OnMapUIStateUpdated onToolStateUpdated;
            public static Events.OnMapUIStateUpdated onSelectedRoomKeyUpdated;
            public static Events.OnMapUIStateUpdated onCurrentFocusFloorUpdated;
            public static Events.OnMapUIStateUpdated onCurrentSelectedTileUpdated;

            public delegate void BlueprintRotationEvent(MapRoomRotation rotation, MapRoomRotation previousRotation);
            public static BlueprintRotationEvent onBlueprintRotationUpdated;

            static Events()
            {
                onMapUIStateUpdated += StateChangeSelectors;
            }

            static void StateChangeSelectors(StateEventPayload payload)
            {
                MapUIState previousState = payload.previousState;
                MapUIState state = payload.state;

                if (previousState.toolState != state.toolState && onToolStateUpdated != null)
                {
                    onToolStateUpdated(payload);
                }

                if (previousState.selectedRoomKey != state.selectedRoomKey && onSelectedRoomKeyUpdated != null)
                {
                    onSelectedRoomKeyUpdated(payload);
                }

                if (previousState.currentFocusFloor != state.currentFocusFloor && onCurrentFocusFloorUpdated != null)
                {
                    onCurrentFocusFloorUpdated(payload);
                }

                if (!previousState.currentSelectedTile.Matches(state.currentSelectedTile))
                {
                    onCurrentSelectedTileUpdated(payload);
                }
            }
        }
    }
}


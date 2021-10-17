using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public partial class MapUIStore
    {
        public static class StateChangeSelectors
        {

            public static Events.OnMapUIStateUpdated onToolStateUpdated;
            public static Events.OnMapUIStateUpdated onSelectedRoomKeyUpdated;
            public static Events.OnMapUIStateUpdated onCurrentFocusFloorUpdated;

            static StateChangeSelectors()
            {
                Events.onMapUIStateUpdated += OnMapUIStateUpdated;
            }

            static void OnMapUIStateUpdated(MapUIStore.StateEventPayload payload)
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
            }
        }
    }
}
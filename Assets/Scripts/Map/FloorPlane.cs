using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.UI
{
    public class FloorPlane : MonoBehaviour
    {
        void Awake()
        {
            MapUIStore.StateChangeSelectors.onCurrentFocusFloorUpdated += OnCurrentFocusFloorUpdated;
        }

        void OnCurrentFocusFloorUpdated(MapUIStore.StateEventPayload payload)
        {
            int currentFocusFloor = payload.state.currentFocusFloor;
            float TILE_SIZE = Stores.Map.MapStore.Constants.TILE_SIZE;

            transform.position = new Vector3(
                transform.position.x,
                (currentFocusFloor * TILE_SIZE) + 0.01f,
                transform.position.z
            );
        }
    }
}

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
            Registry.Stores.MapUI.onCurrentFocusFloorUpdated += OnCurrentFocusFloorUpdated;
        }

        void OnCurrentFocusFloorUpdated(int currentFocusFloor)
        {
            float TILE_SIZE = Stores.Map.Constants.TILE_SIZE;

            transform.position = new Vector3(
                transform.position.x,
                (currentFocusFloor * TILE_SIZE) + 0.01f,
                transform.position.z
            );
        }
    }
}

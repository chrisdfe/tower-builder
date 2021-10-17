using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.UI
{

    public class MapCursor : MonoBehaviour
    {
        public static float CURSOR_SIZE = 0.8f;

        bool isVisible = true;
        bool isEnabled = true;

        void Awake()
        {
            transform.localScale = new Vector3(CURSOR_SIZE, CURSOR_SIZE, CURSOR_SIZE);
        }

        void Update() { }

        public void SetCurrentTile(CellCoordinates cellCoordinates)
        {
            transform.localPosition = new Vector3(
                MapCellHelpers.RoundToNearestTile(cellCoordinates.x),
                (
                    MapCellHelpers.RoundToNearestTile(Registry.storeRegistry.mapUIStore.state.currentFocusFloor) +
                    (CURSOR_SIZE / 2)
                ),
                MapCellHelpers.RoundToNearestTile(cellCoordinates.z)
            );
        }

        public void Hide()
        {
            isVisible = false;
            Disable();
            // TODO - set material
        }

        public void Show()
        {
            isVisible = true;
            Enable();
            // TODO - set material
        }

        public void Disable()
        {
            isEnabled = false;
        }

        public void Enable()
        {
            isEnabled = true;
        }
    }
}

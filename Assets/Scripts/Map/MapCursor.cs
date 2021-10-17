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

        public bool isVisible { get; private set; } = true;
        public bool isEnabled { get; private set; } = true;

        Material material;

        void Awake()
        {
            transform.localScale = new Vector3(CURSOR_SIZE, CURSOR_SIZE, CURSOR_SIZE);
            material = GetComponent<Renderer>().material;
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
            Debug.Log("hiding mapCursor");
            isVisible = false;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
        }

        public void Show()
        {
            Debug.Log("showing mapCursor");
            isVisible = true;
            material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
        }

        public void Disable()
        {
            isEnabled = false;
            Hide();
        }

        public void Enable()
        {
            isEnabled = true;
            Show();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class DebugPanelManager : MonoBehaviour
    {
        Text currentSelectedCellText;

        void Awake()
        {
            Registry.Stores.MapUI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;

            currentSelectedCellText = transform.Find("CurrentSelectedCellText").GetComponent<Text>();
            currentSelectedCellText.text = "";
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates)
        {
            SetCurrentSelectedCellText();
        }

        void SetCurrentSelectedCellText()
        {
            CellCoordinates currentSelectedCell = Registry.Stores.MapUI.currentSelectedCell;
            currentSelectedCellText.text = $"x: {currentSelectedCell.x}, floor: {currentSelectedCell.floor}";
        }
    }
}
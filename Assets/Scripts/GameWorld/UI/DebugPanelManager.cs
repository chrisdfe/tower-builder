using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;

using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class DebugPanelManager : MonoBehaviour
    {
        Text currentSelectedCellText;

        void Awake()
        {
            Registry.Stores.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;

            currentSelectedCellText = transform.Find("CurrentSelectedCellText").GetComponent<Text>();
            currentSelectedCellText.text = "";
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates)
        {
            SetCurrentSelectedCellText();
        }

        void SetCurrentSelectedCellText()
        {
            CellCoordinates currentSelectedCell = Registry.Stores.UI.currentSelectedCell;
            currentSelectedCellText.text = $"x: {currentSelectedCell.x}, floor: {currentSelectedCell.floor}";
        }
    }
}
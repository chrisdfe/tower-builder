using System.Collections;
using System.Collections.Generic;
using System.IO;
using TowerBuilder;
using TowerBuilder.Stores;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class DebugPanelManager : MonoBehaviour
    {

        Text currentSelectedCellText;
        Button debugButton;

        void Awake()
        {
            Registry.Stores.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;

            currentSelectedCellText = transform.Find("CurrentSelectedCellText").GetComponent<Text>();
            currentSelectedCellText.text = "";

            debugButton = transform.Find("DebugButton").GetComponent<Button>();
            debugButton.onClick.AddListener(OnDebugButtonClick);
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

        void OnDebugButtonClick()
        {
            // string jsonifiedRoomStore = JsonSerializer.Serialize(Registry.Stores.Rooms);
            // Debug.Log(jsonifiedRoomStore);

            // JsonWriter.WriteString("test.json", jsonifiedRoomStore);
            // JsonWriter.ReadString();
        }
    }
}
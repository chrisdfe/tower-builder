using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Buildings;
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
            Registry.appState.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;

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
            CellCoordinates currentSelectedCell = Registry.appState.UI.currentSelectedCell;
            currentSelectedCellText.text = $"x: {currentSelectedCell.x}, floor: {currentSelectedCell.floor}";
        }

        void OnDebugButtonClick()
        {
            if (Registry.appState.Rooms.buildings.Count > 0)
            {
                Building building = Registry.appState.Rooms.buildings.buildings[0];

                if (building.roomList.Count > 0)
                {
                    Room room = building.roomList.rooms[0];

                    Debug.Log("serializing room: " + room);

                    string jsonifiedRoomStore = JsonConvert.SerializeObject(room);
                    Debug.Log(jsonifiedRoomStore);
                }
            }


            // JsonWriter.WriteString("test.json", jsonifiedRoomStore);
            // JsonWriter.ReadString();
        }
    }
}
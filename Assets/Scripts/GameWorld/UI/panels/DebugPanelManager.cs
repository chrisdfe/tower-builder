using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Vehicles;
using TowerBuilder.Systems;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class DebugPanelManager : MonoBehaviour
    {

        Text currentSelectedCellText;
        Text vehicleCountText;
        Text roomCountText;
        Text roomConnectionsText;
        Text selectionBoxText;

        void Awake()
        {
            Registry.appState.UI.events.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.appState.UI.events.onSelectionBoxUpdated += OnSelectionBoxUpdated;

            Registry.appState.Vehicles.events.onListUpdated += OnVehicleListUpdated;

            Registry.appState.Rooms.events.onListUpdated += OnRoomListUpdated;

            currentSelectedCellText = TransformUtils.FindDeepChild(transform, "CurrentSelectedCellText").GetComponent<Text>();
            currentSelectedCellText.text = "";

            selectionBoxText = TransformUtils.FindDeepChild(transform, "SelectionBoxText").GetComponent<Text>();
            selectionBoxText.text = "";

            vehicleCountText = TransformUtils.FindDeepChild(transform, "VehicleCountText").GetComponent<Text>();
            vehicleCountText.text = "";

            roomCountText = TransformUtils.FindDeepChild(transform, "RoomCountText").GetComponent<Text>();
            roomCountText.text = "";

            roomConnectionsText = TransformUtils.FindDeepChild(transform, "RoomConnectionsText").GetComponent<Text>();
            roomConnectionsText.text = "";

            SetCurrentSelectedCellText();
            SetSelectionBoxText();
            SetVehicleCountText();
            SetRoomCountText();
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates)
        {
            SetCurrentSelectedCellText();
        }

        void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            SetSelectionBoxText();
        }

        void OnVehicleListUpdated(VehicleList vehicles)
        {
            SetVehicleCountText();
        }

        void OnRoomListUpdated(RoomList roomList)
        {
            SetRoomCountText();
        }

        void SetCurrentSelectedCellText()
        {
            CellCoordinates currentSelectedCell = Registry.appState.UI.currentSelectedCell;
            currentSelectedCellText.text = $"x: {currentSelectedCell.x}, floor: {currentSelectedCell.floor}\n"
                + $"entityStack: {Registry.appState.UI.currentSelectedCellEntityList.Count}";
        }

        void SetSelectionBoxText()
        {
            SelectionBox selectionBox = Registry.appState.UI.selectionBox;

            selectionBoxText.text = ($"selectionBox\n"
            + $"    start: {selectionBox.start}\n"
            + $"    end: {selectionBox.end}\n"
            + $"    topLeft: {selectionBox.cellCoordinatesList.topLeftCoordinates}\n"
            + $"    bottomRight: {selectionBox.cellCoordinatesList.bottomRightCoordinates}");
        }

        void SetVehicleCountText()
        {
            VehicleList allVehicles = Registry.appState.Vehicles.list;
            vehicleCountText.text = $"Vehicles: {allVehicles.Count}";
        }

        void SetRoomCountText()
        {
            RoomList roomsList = Registry.appState.Rooms.list;
            roomCountText.text = $"Rooms: {roomsList.Count}";
        }
    }
}
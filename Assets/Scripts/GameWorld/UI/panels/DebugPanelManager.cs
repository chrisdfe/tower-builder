using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Vehicles;
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
            Registry.appState.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.appState.UI.onSelectionBoxUpdated += OnSelectionBoxUpdated;

            // Registry.appState.Entities.Vehicles.onListUpdated += OnVehicleListUpdated;

            // Registry.appState.Entities.Rooms.onListUpdated += OnRoomListUpdated;

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

        void OnVehicleListUpdated(ListWrapper<Vehicle> vehicles)
        {
            SetVehicleCountText();
        }

        void OnRoomListUpdated(ListWrapper<Room> roomList)
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
            // ListWrapper<Vehicle> allVehicles = Registry.appState.Entities.Vehicles.list;
            // vehicleCountText.text = $"Vehicles: {allVehicles.Count}";
        }

        void SetRoomCountText()
        {
            // ListWrapper<Room> roomsList = Registry.appState.Entities.Rooms.list;
            // roomCountText.text = $"Rooms: {roomsList.Count}";
        }
    }
}
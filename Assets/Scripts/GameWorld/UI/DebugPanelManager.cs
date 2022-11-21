using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
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

            Registry.appState.Vehicles.events.onVehicleListUpdated += OnVehicleListUpdated;

            Registry.appState.Rooms.events.onRoomListUpdated += OnRoomListUpdated;
            Registry.appState.Rooms.events.onRoomConnectionsUpdated += OnRoomConnectionsUpdated;

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
            SetRoomConnectionsText();
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates)
        {
            SetCurrentSelectedCellText();
        }

        void OnSelectionBoxUpdated(SelectionBox selectionBox)
        {
            SetSelectionBoxText();
        }

        void OnVehicleListUpdated(List<Vehicle> vehicles)
        {
            SetVehicleCountText();
        }

        void OnRoomListUpdated(RoomList roomList)
        {
            SetRoomCountText();
        }

        void OnRoomConnectionsUpdated(RoomConnections roomConnection)
        {
            SetRoomConnectionsText();
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
            List<Vehicle> allVehicles = Registry.appState.Vehicles.vehicleList;
            vehicleCountText.text = $"Vehicles: {allVehicles.Count}";
        }

        void SetRoomCountText()
        {
            RoomList roomsList = Registry.appState.Rooms.roomList;
            roomCountText.text = $"Rooms: {roomsList.Count}";
        }

        void SetRoomConnectionsText()
        {
            RoomConnections roomConnections = Registry.appState.Rooms.roomConnections;
            roomConnectionsText.text = $"Room Connections: {roomConnections.Count}";
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Buildings;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.Systems;
using TowerBuilder.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TowerBuilder.GameWorld.UI
{
    public class DebugPanelManager : MonoBehaviour
    {

        Text currentSelectedCellText;
        Text buildingCountText;
        Text roomCountText;

        void Awake()
        {
            Registry.appState.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.appState.buildings.buildingList.onItemsChanged += OnBuildingListUpdated;
            Registry.appState.Rooms.roomList.onItemsChanged += OnRoomListUpdated;

            currentSelectedCellText = TransformUtils.FindDeepChild(transform, "CurrentSelectedCellText").GetComponent<Text>();
            currentSelectedCellText.text = "";

            buildingCountText = TransformUtils.FindDeepChild(transform, "BuildingCountText").GetComponent<Text>();
            buildingCountText.text = "";

            roomCountText = TransformUtils.FindDeepChild(transform, "RoomCountText").GetComponent<Text>();
            roomCountText.text = "";

            SetCurrentSelectedCellText();
            SetBuildingCountText();
            SetRoomCountText();
        }

        void OnCurrentSelectedCellUpdated(CellCoordinates cellCoordinates)
        {
            SetCurrentSelectedCellText();
        }

        void OnBuildingListUpdated(List<Building> buildings)
        {
            SetBuildingCountText();
        }

        void OnRoomListUpdated(List<Room> roomList)
        {
            SetRoomCountText();
        }

        void SetCurrentSelectedCellText()
        {
            CellCoordinates currentSelectedCell = Registry.appState.UI.currentSelectedCell;
            currentSelectedCellText.text = $"x: {currentSelectedCell.x}, floor: {currentSelectedCell.floor}";
        }

        void SetBuildingCountText()
        {
            ResourceList<Building> allBuildings = Registry.appState.buildings.buildingList;
            buildingCountText.text = $"Buildings: {allBuildings.Count}";
        }

        void SetRoomCountText()
        {
            RoomList roomsList = Registry.appState.Rooms.roomList;
            roomCountText.text = $"Rooms: {roomsList.Count}";
        }

    }
}
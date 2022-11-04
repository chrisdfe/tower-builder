using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.GameWorld.Rooms
{
    public class GameWorldRooms : MonoBehaviour
    {
        public GameWorldRoomList roomList;

        public void Awake()
        {
            Registry.appState.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
        }

        public void OnCurrentSelectedCellUpdated(CellCoordinates clelCoordinates)
        {

        }
    }
}
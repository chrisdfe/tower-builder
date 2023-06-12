using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState.Entities;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups.Buildings
{
    public class State : EntityGroupStateSlice
    {
        public class Input
        {
            public ListWrapper<Building> buildingList;
        }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        /* 
            Public Interface
        */
        public void AddToBuilding(Building building, Room room)
        {
            vehicle.Add(room);

            // onVehicleRoomAdded?.Invoke(vehicle, room);
        }

        public void RemoveRoomFromVehicle(Building building, Room room)
        {
            vehicle.Remove(room);

            // onVehicleRoomRemoved?.Invoke(vehicle, room);
        }

        /*
            Event Handlers
        */
        void OnRoomsAdded(ListWrapper<Room> roomList)
        {
        }

        void OnRoomsBuilt(ListWrapper<Room> roomList)
        {
        }

        void OnRoomsRemoved(ListWrapper<Room> roomList)
        {
        }
    }
}
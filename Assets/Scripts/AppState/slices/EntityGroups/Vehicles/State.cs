using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState.Entities;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.EntityGroups.Vehicles
{
    public class State : EntityGroupStateSlice
    {
        public class Input
        {
            public ListWrapper<Vehicle> vehicleList;
        }

        public ItemEvent<Vehicle> onVehicleIsPilotedUpdated;

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public override void Setup()
        {
            base.Setup();

            // appState.Entities.Rooms.onItemsAdded += OnRoomsAdded;
            // appState.Entities.Rooms.onItemsBuilt += OnRoomsBuilt;
            // appState.Entities.Rooms.onItemsRemoved += OnRoomsRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();

            // appState.Entities.Rooms.onItemsAdded -= OnRoomsAdded;
            // appState.Entities.Rooms.onItemsBuilt -= OnRoomsBuilt;
            // appState.Entities.Rooms.onItemsRemoved -= OnRoomsRemoved;
        }

        /* 
            Public Interface
        */
        public void AddRoomToVehicle(Vehicle vehicle, Room room)
        {
            vehicle.roomList.Add(room);

            // onVehicleRoomAdded?.Invoke(vehicle, room);
        }

        public void RemoveRoomFromVehicle(Vehicle vehicle, Room room)
        {
            vehicle.roomList.Remove(room);

            // onVehicleRoomRemoved?.Invoke(vehicle, room);
        }

        public void SetVehicleIsPiloted(Vehicle vehicle, bool isPiloted)
        {
            // vehicle.isPiloted = isPiloted;
            // onVehicleIsPilotedUpdated?.Invoke(vehicle);
        }

        /*
            Event Handlers
        */
        void OnRoomsAdded(ListWrapper<Room> roomList)
        {
        }

        void OnRoomsBuilt(ListWrapper<Room> roomList)
        {
            // roomList.ForEach(room =>
            // {
            //     List<Room> perimeterRooms = appState.Entities.Rooms.queries.FindPerimeterRooms(room);

            //     if (perimeterRooms.Count > 0)
            //     {
            //         // TODO here - if perimeterRooms has more than 1 then combine them
            //         Vehicle vehicle = queries.FindVehicleByRoom(perimeterRooms[0]);

            //         AddRoomToVehicle(vehicle, room);
            //     }
            //     else
            //     {
            //         Vehicle vehicle = new Vehicle(Registry.Definitions.Entities.Vehicles.defaultDefinition as VehicleDefinition);

            //         vehicle.roomList.Add(room);
            //         Add(vehicle);
            //     }
            // });
        }

        void OnRoomsRemoved(ListWrapper<Room> roomList)
        {
            // roomList.ForEach(room =>
            // {
            //     Vehicle vehicleContainingRoom = queries.FindVehicleByRoom(room);

            //     if (vehicleContainingRoom == null) return;

            //     RemoveRoomFromVehicle(vehicleContainingRoom, room);

            //     if (vehicleContainingRoom.roomList.Count == 0)
            //     {
            //         Remove(vehicleContainingRoom);
            //     }
            // });
        }
    }
}
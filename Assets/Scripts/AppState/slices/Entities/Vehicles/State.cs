using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState.Entities;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Entities.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Vehicles
{
    public class State : EntityStateSlice<Vehicle, State.Events>
    {
        public class Input
        {
            public ListWrapper<Vehicle> vehicleList;
        }

        public new class Events : EntityStateSlice<Vehicle, State.Events>.Events
        {
            public ItemEvent<Vehicle> onVehicleIsPilotedUpdated;

            public delegate void VehicleRoomEvent(Vehicle vehicle, Room room);
            public VehicleRoomEvent onVehicleRoomAdded;
            public VehicleRoomEvent onVehicleRoomRemoved;
        }

        public new class Queries : EntityStateSlice<Vehicle, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }

            public Vehicle FindVehicleByRoom(Room room) =>
                state.list.items.Find(otherVehicle => otherVehicle.roomList.Contains(room));

            // TODO - don't use items[0] I think?
            public Vehicle FindVehicleByFurniture(Furniture furniture) =>
                FindVehicleByRoom(appState.Entities.Rooms.queries.FindRoomAtCell(furniture.cellCoordinatesList.items[0]));
        }

        public new Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        public override void Setup()
        {
            base.Setup();
            appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;
        }

        public override void Teardown()
        {
            base.Teardown();
            appState.Entities.Rooms.events.onItemsAdded -= OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsBuilt -= OnRoomsBuilt;
            appState.Entities.Rooms.events.onItemsRemoved -= OnRoomsRemoved;
        }

        /* 
            Public Interface
        */
        public void AddRoomToVehicle(Vehicle vehicle, Room room)
        {
            vehicle.roomList.Add(room);

            events.onVehicleRoomAdded?.Invoke(vehicle, room);
        }

        public void RemoveRoomFromVehicle(Vehicle vehicle, Room room)
        {
            vehicle.roomList.Remove(room);

            events.onVehicleRoomRemoved?.Invoke(vehicle, room);
        }

        public void SetVehicleIsPiloted(Vehicle vehicle, bool isPiloted)
        {
            vehicle.isPiloted = isPiloted;
            events.onVehicleIsPilotedUpdated?.Invoke(vehicle);
        }

        /*
            Event Handlers
        */
        void OnRoomsAdded(ListWrapper<Room> roomList)
        {
        }

        void OnRoomsBuilt(ListWrapper<Room> roomList)
        {
            roomList.ForEach(room =>
            {
                List<Room> perimeterRooms = appState.Entities.Rooms.queries.FindPerimeterRooms(room);

                if (perimeterRooms.Count > 0)
                {
                    // TODO here - if perimeterRooms has more than 1 then combine them
                    Vehicle vehicle = queries.FindVehicleByRoom(perimeterRooms[0]);

                    AddRoomToVehicle(vehicle, room);
                }
                else
                {
                    Vehicle vehicle = new Vehicle(Registry.Definitions.Entities.Vehicles.defaultDefinition as VehicleDefinition);

                    vehicle.roomList.Add(room);
                    Add(vehicle);
                }
            });
        }

        void OnRoomsRemoved(ListWrapper<Room> roomList)
        {
            roomList.ForEach(room =>
            {
                Vehicle vehicleContainingRoom = queries.FindVehicleByRoom(room);

                if (vehicleContainingRoom == null) return;

                RemoveRoomFromVehicle(vehicleContainingRoom, room);

                if (vehicleContainingRoom.roomList.Count == 0)
                {
                    Remove(vehicleContainingRoom);
                }
            });
        }
    }
}
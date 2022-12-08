using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Vehicles
{
    using VehicleListStateSlice = ListStateSlice<VehicleList, Vehicle, State.Events>;

    public class State : VehicleListStateSlice
    {
        public class Input
        {
            public VehicleList vehicleList;
        }

        public new class Events : VehicleListStateSlice.Events
        {
            public delegate void VehicleRoomEvent(Vehicle vehicle, Room room);
            public VehicleRoomEvent onVehicleRoomAdded;
            public VehicleRoomEvent onVehicleRoomRemoved;
        }

        public class Queries
        {
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }

            public Vehicle FindVehicleByRoom(Room room) =>
                state.list.items.Find(otherVehicle => otherVehicle.roomList.Contains(room));

            // TODO - don't use items[0] I think?
            public Vehicle FindVehicleByFurniture(Furniture furniture) =>
                FindVehicleByRoom(appState.Entities.Rooms.queries.FindRoomAtCell(furniture.cellCoordinatesList.items[0]));
        }

        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.vehicleList ?? new VehicleList();

            queries = new Queries(appState, this);
        }

        public override void Setup()
        {
            appState.Entities.Rooms.events.onItemsAdded += OnRoomsAdded;
            appState.Entities.Rooms.events.onItemsBuilt += OnRoomsBuilt;
            appState.Entities.Rooms.events.onItemsRemoved += OnRoomsRemoved;
        }

        public override void Teardown()
        {
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

            if (events.onVehicleRoomAdded != null)
            {
                events.onVehicleRoomAdded(vehicle, room);
            }
        }

        public void RemoveRoomFromVehicle(Vehicle vehicle, Room room)
        {
            vehicle.roomList.Remove(room);

            if (events.onVehicleRoomRemoved != null)
            {
                events.onVehicleRoomRemoved(vehicle, room);
            }
        }

        /* 
            Event Handlers
        */
        void OnRoomsAdded(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                if (room.isInBlueprintMode) return;

                List<Room> perimeterRooms = appState.Entities.Rooms.queries.FindPerimeterRooms(room);

                if (perimeterRooms.Count > 0)
                {
                    // TODO here - if perimeterRooms has more than 1 then combine them
                    Vehicle vehicle = queries.FindVehicleByRoom(perimeterRooms[0]);
                    AddRoomToVehicle(vehicle, room);
                }
                else
                {
                    Vehicle vehicle = new Vehicle();
                    vehicle.roomList.Add(room);
                    Add(vehicle);
                }
            });
        }

        void OnRoomsBuilt(RoomList roomList)
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
                    Vehicle vehicle = new Vehicle();
                    vehicle.roomList.Add(room);
                    Add(vehicle);
                }
            });
        }

        void OnRoomsRemoved(RoomList roomList)
        {
            roomList.ForEach(room =>
            {
                Vehicle vehicleContainingRoom = queries.FindVehicleByRoom(room);

                if (vehicleContainingRoom == null) return;

                RemoveRoomFromVehicle(vehicleContainingRoom, room);

                if (vehicleContainingRoom.roomList.Count == 0)
                {
                    Add(vehicleContainingRoom);
                }
            });
        }
    }
}
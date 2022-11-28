using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Vehicles;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Vehicles
{
    public class State : StateSlice
    {
        public class Input
        {
            public List<Vehicle> vehicleList;
        }

        public class Events
        {
            public delegate void VehicleListEvent(List<Vehicle> vehicleList);
            public VehicleListEvent onVehicleListUpdated;

            public delegate void VehicleEvent(Vehicle vehicle);
            public VehicleEvent onVehicleAdded;
            public VehicleEvent onVehicleRemoved;

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

            public Vehicle FindVehicleByRoom(Room room)
            {
                foreach (Vehicle vehicle in state.vehicleList)
                {
                    if (vehicle.roomList.Contains(room))
                    {
                        return vehicle;
                    }
                }

                return null;
            }

            public Vehicle FindVehicleByFurniture(Furniture furniture)
            {
                Room furnitureRoom = appState.Rooms.queries.FindRoomAtCell(furniture.cellCoordinates);

                foreach (Vehicle vehicle in state.vehicleList)
                {
                    if (vehicle.roomList.Contains(furnitureRoom))
                    {
                        return vehicle;
                    }
                }

                return null;
            }
        }

        public List<Vehicle> vehicleList = new List<Vehicle>();

        public Events events;
        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            vehicleList = input.vehicleList ?? new List<Vehicle>();

            events = new Events();
            queries = new Queries(appState, this);

            Setup();
        }

        public void Setup()
        {
            appState.Rooms.events.onRoomAdded += OnRoomAdded;
            appState.Rooms.events.onRoomBuilt += OnRoomBuilt;
            appState.Rooms.events.onRoomRemoved += OnRoomRemoved;
        }

        public void Teardown()
        {
            appState.Rooms.events.onRoomAdded -= OnRoomAdded;
            appState.Rooms.events.onRoomBuilt -= OnRoomBuilt;
            appState.Rooms.events.onRoomRemoved -= OnRoomRemoved;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicleList.Add(vehicle);

            if (events.onVehicleAdded != null)
            {
                events.onVehicleAdded(vehicle);
            }

            if (events.onVehicleListUpdated != null)
            {
                events.onVehicleListUpdated(vehicleList);
            }
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            vehicleList.Remove(vehicle);

            if (events.onVehicleRemoved != null)
            {
                events.onVehicleRemoved(vehicle);
            }

            if (events.onVehicleListUpdated != null)
            {
                events.onVehicleListUpdated(vehicleList);
            }
        }

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
        void OnRoomAdded(Room room)
        {
            if (room.isInBlueprintMode) return;

            List<Room> perimeterRooms = appState.Rooms.queries.FindPerimeterRooms(room);

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
                AddVehicle(vehicle);
            }
        }

        void OnRoomBuilt(Room room)
        {
            List<Room> perimeterRooms = appState.Rooms.queries.FindPerimeterRooms(room);

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
                AddVehicle(vehicle);
            }
        }

        void OnRoomRemoved(Room room)
        {
            Vehicle vehicleContainingRoom = queries.FindVehicleByRoom(room);

            if (vehicleContainingRoom == null) return;

            RemoveRoomFromVehicle(vehicleContainingRoom, room);

            if (vehicleContainingRoom.roomList.Count == 0)
            {
                RemoveVehicle(vehicleContainingRoom);
            }
        }
    }
}
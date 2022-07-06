using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Blueprints;
using TowerBuilder.DataTypes.Rooms.Buildings;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Validators;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.State.Rooms
{
    [Serializable]
    public class State
    {
        public struct Input
        {
            public BuildingList buildings;
            public RoomConnections roomConnections;
        }

        public BuildingList buildings { get; private set; } = new BuildingList();
        public delegate void BuildingConstructionEvent(Building building);
        public BuildingConstructionEvent onBuildingAdded;
        public BuildingConstructionEvent onBuildingDestroyed;

        public delegate void RoomAddedEvent(Room room);
        public RoomAddedEvent onRoomAdded;

        public delegate void RoomDestroyedEvent(Room room);
        public RoomDestroyedEvent onRoomDestroyed;

        public delegate void RoomBlockDestroyedEvent(RoomCells roomBlock);
        public RoomBlockDestroyedEvent onRoomBlockDestroyed;

        public RoomConnections roomConnections { get; private set; } = new RoomConnections();

        public delegate void RoomConnectionsEvent(RoomConnections roomConnections);
        public RoomConnectionsEvent onRoomConnectionsUpdated;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            buildings = input.buildings ?? new BuildingList();
            roomConnections = input.roomConnections ?? new RoomConnections();
        }

        // Buildings
        public void AddBuilding(Building building)
        {
            buildings.Add(building);

            if (onBuildingAdded != null)
            {
                onBuildingAdded(building);
            }
        }

        public void DestroyBuilding(Building building)
        {
            buildings.Remove(building);

            if (onBuildingDestroyed != null)
            {
                onBuildingDestroyed(building);
            }
        }

        // Rooms
        public void AddRoom(Room room, Building building)
        {
            if (room == null)
            {
                return;
            }

            building.AddRoom(room);

            room.OnBuild();

            if (onRoomAdded != null)
            {
                onRoomAdded(room);
            }
        }

        public void AttemptToAddRoom(Blueprint blueprint, RoomConnections roomConnections)
        {
            List<RoomValidationError> validationErrors = blueprint.Validate(Registry.appState);

            if (validationErrors.Count > 0)
            {
                // TODO - these should be unique messages - right now they are not
                foreach (RoomValidationError validationError in validationErrors)
                {
                    Registry.appState.Notifications.createNotification(validationError.message);
                }
                return;
            }

            // 
            Registry.appState.Wallet.SubtractBalance(blueprint.room.GetPrice());

            // Decide whether to create a new room or to add to an existing one
            List<Room> roomsToCombineWith = FindRoomsToCombineWith(blueprint.room);

            Room newRoom = blueprint.room;
            Building building;

            if (roomsToCombineWith.Count > 0)
            {
                building = buildings.FindBuildingByRoom(roomsToCombineWith[0]);

                foreach (Room otherRoom in roomsToCombineWith)
                {
                    newRoom.AddBlocks(otherRoom.blocks);

                    DestroyRoom(otherRoom, false);
                }

                // TODO - this might not be the best place to call this
                newRoom.Reset();
            }
            else
            {
                List<CellCoordinates> perimeterRoomCellCoordinates = newRoom.roomCells.GetPerimeterCellCoordinates();
                Debug.Log("perimeter room cells: ");
                Debug.Log(perimeterRoomCellCoordinates.Count);


                // find the first cellcoordinates that are inside of a room
                List<RoomCell> occupiedPerimeterRoomCells = new List<RoomCell>();

                Room perimeterRoom = null;

                foreach (CellCoordinates coordinates in perimeterRoomCellCoordinates)
                {
                    perimeterRoom = Registry.appState.Rooms.buildings.FindRoomAtCell(coordinates);
                    if (perimeterRoom != null)
                    {
                        break;
                    }
                }

                //  TODO somewhere here - also check if buildings should be combined???

                if (perimeterRoom != null)
                {
                    building = buildings.FindBuildingByRoom(perimeterRoom);
                }
                else
                {
                    building = new Building();
                    buildings.Add(building);
                }

            }

            Debug.Log("building count: ");
            Debug.Log(buildings.Count);
            AddRoom(newRoom, building);

            if (roomConnections.connections.Count > 0)
            {
                AddRoomConnections(roomConnections);
            }
        }

        public void DestroyRoom(Room room, bool checkForBuildingDestroy = true)
        {
            if (room == null)
            {
                return;
            }

            RemoveRoomConnectionsForRoom(room);

            if (checkForBuildingDestroy)
            {
                Building building = buildings.FindBuildingByRoom(room);

                Building buildingContainingRoom = buildings.FindBuildingByRoom(room);
                building.RemoveRoom(room);

                Debug.Log("building count: ");
                Debug.Log(buildings.Count);

                if (building.roomList.rooms.Count == 0)
                {
                    DestroyBuilding(building);
                }
            }

            if (onRoomDestroyed != null)
            {
                onRoomDestroyed(room);
            }
        }

        public void DestroyRoomBlock(Room room, RoomCells roomBlock)
        {
            if (room == null) return;

            // TODO - check if doing this is going to divide the room into 2
            // if so, create another room right here

            room.RemoveBlock(roomBlock);
            room.ResetRoomCellOrientations();

            // TODO - destroy connections block may have had

            if (room.blocks.Count == 0)
            {
                DestroyRoom(room);
            }
            else
            {
                if (onRoomBlockDestroyed != null)
                {
                    onRoomBlockDestroyed(roomBlock);
                }
            }
        }

        public void AddRoomConnections(RoomConnections newRoomConnections)
        {
            roomConnections.Add(newRoomConnections);

            if (onRoomConnectionsUpdated != null)
            {
                onRoomConnectionsUpdated(roomConnections);
            }
        }

        public void RemoveRoomConnectionsForRoom(Room roomBeingDestroyed)
        {
            roomConnections.RemoveConnectionsForRoom(roomBeingDestroyed);

            if (onRoomConnectionsUpdated != null)
            {
                onRoomConnectionsUpdated(roomConnections);
            }
        }

        List<Room> FindRoomsToCombineWith(Room room)
        {
            List<Room> result = new List<Room>();

            if (room.roomTemplate.resizability.Matches(RoomResizability.Inflexible()))
            {
                return result;
            }

            if (room.roomTemplate.resizability.x)
            {
                //  Check on either side
                foreach (int floor in room.roomCells.GetFloorRange())
                {
                    Room leftRoom = buildings.FindRoomAtCell(new CellCoordinates(
                        room.roomCells.GetLowestX() - 1,
                        floor
                    ));

                    Room rightRoom = buildings.FindRoomAtCell(new CellCoordinates(
                        room.roomCells.GetHighestX() + 1,
                        floor
                    ));

                    foreach (Room otherRoom in new Room[] { leftRoom, rightRoom })
                    {
                        if (
                            otherRoom != null &&
                            otherRoom.roomTemplate.key == room.roomTemplate.key &&
                            !result.Contains(otherRoom)
                        )
                        {
                            result.Add(otherRoom);
                        }
                    }
                }
            }

            if (room.roomTemplate.resizability.floor)
            {
                //  Check on floors above and below
                foreach (int x in room.roomCells.GetXRange())
                {
                    Room aboveRoom = buildings.FindRoomAtCell(new CellCoordinates(
                        x,
                        room.roomCells.GetHighestFloor() + 1
                    ));

                    Room belowRoom = buildings.FindRoomAtCell(new CellCoordinates(
                        x,
                        room.roomCells.GetLowestFloor() - 1
                    ));

                    foreach (Room otherRoom in new Room[] { aboveRoom, belowRoom })
                    {
                        if (
                            otherRoom != null &&
                            otherRoom.roomTemplate.key == room.roomTemplate.key &&
                            !result.Contains(otherRoom)
                        )
                        {
                            result.Add(otherRoom);
                        }
                    }
                }
            }

            return result;
        }
    }
}

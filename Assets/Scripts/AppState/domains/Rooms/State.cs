using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Buildings;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Blueprints;
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
            public RoomConnections roomConnections;
            public RoomList roomList;
        }

        public RoomList roomList;
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
            roomConnections = input.roomConnections ?? new RoomConnections();
            roomList = input.roomList ?? new RoomList();
        }

        // Rooms
        public void AddRoom(Room room)
        {
            if (room == null)
            {
                return;
            }

            roomList.Add(room);
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
            Registry.appState.Wallet.SubtractBalance(blueprint.room.price);

            // Decide whether to create a new room or to add to an existing one
            List<Room> roomsToCombineWith = FindRoomsToCombineWith(blueprint.room);

            Room newRoom = blueprint.room;
            Building building;

            if (roomsToCombineWith.Count > 0)
            {
                building = FindBuildingByRoom(roomsToCombineWith[0]);

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

                // find the first cellcoordinates that are inside of a room
                List<RoomCell> occupiedPerimeterRoomCells = new List<RoomCell>();

                Room perimeterRoom = null;

                foreach (CellCoordinates coordinates in perimeterRoomCellCoordinates)
                {
                    perimeterRoom = FindRoomAtCell(coordinates);
                    if (perimeterRoom != null)
                    {
                        break;
                    }
                }

                //  TODO somewhere here - also check if buildings should be combined???

                if (perimeterRoom != null)
                {
                    building = FindBuildingByRoom(perimeterRoom);
                }
                else
                {
                    building = new Building();
                    Registry.appState.buildings.AddBuilding(building);
                }

            }

            AddRoom(newRoom);

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
                Building building = FindBuildingByRoom(room);

                Building buildingContainingRoom = FindBuildingByRoom(room);
                roomList.Remove(room);

                if (roomList.rooms.Count == 0)
                {
                    Registry.appState.buildings.DestroyBuilding(building);
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

        // Queries
        public Building FindBuildingByRoom(Room room)
        {
            foreach (Building building in Registry.appState.buildings.buildings.buildings)
            {
                if (room.buildingId == building.id)
                {
                    return building;
                }
            }
            return null;
        }

        public RoomList FindRoomsInBuilding(Building building)
        {
            RoomList buildingRooms = new RoomList();

            foreach (Room room in roomList.rooms)
            {
                if (room.buildingId == building.id)
                {
                    buildingRooms.Add(room);
                }
            }

            return buildingRooms;
        }

        public Room FindRoomAtCell(CellCoordinates cellCoordinates)
        {
            return roomList.FindRoomAtCell(cellCoordinates);
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

            if (room.resizability.Matches(RoomResizability.Inflexible()))
            {
                return result;
            }

            if (room.resizability.x)
            {
                //  Check on either side
                foreach (int floor in room.roomCells.GetFloorRange())
                {
                    Room leftRoom = FindRoomAtCell(new CellCoordinates(
                        room.roomCells.GetLowestX() - 1,
                        floor
                    ));

                    Room rightRoom = FindRoomAtCell(new CellCoordinates(
                        room.roomCells.GetHighestX() + 1,
                        floor
                    ));

                    foreach (Room otherRoom in new Room[] { leftRoom, rightRoom })
                    {
                        if (
                            otherRoom != null &&
                            otherRoom.key == room.key &&
                            !result.Contains(otherRoom)
                        )
                        {
                            result.Add(otherRoom);
                        }
                    }
                }
            }

            if (room.resizability.floor)
            {
                //  Check on floors above and below
                foreach (int x in room.roomCells.GetXRange())
                {
                    Room aboveRoom = FindRoomAtCell(new CellCoordinates(
                        x,
                        room.roomCells.GetHighestFloor() + 1
                    ));

                    Room belowRoom = FindRoomAtCell(new CellCoordinates(
                        x,
                        room.roomCells.GetLowestFloor() - 1
                    ));

                    foreach (Room otherRoom in new Room[] { aboveRoom, belowRoom })
                    {
                        if (
                            otherRoom != null &&
                            otherRoom.key == room.key &&
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

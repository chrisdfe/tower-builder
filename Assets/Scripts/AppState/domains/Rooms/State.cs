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
            public RoomList roomList;
            public RoomConnections roomConnections;
        }

        public RoomList roomList { get; private set; } = new RoomList();
        public RoomConnections roomConnections { get; private set; } = new RoomConnections();

        public delegate void RoomEvent(Room room);
        public RoomEvent onRoomAdded;
        public RoomEvent onRoomRemoved;

        public delegate void RoomListEvent(RoomList roomList);
        public RoomListEvent onRoomListUpdated;

        public delegate void RoomCellsEvent(Room room, RoomCells cells);
        public RoomCellsEvent onRoomCellsAdded;
        public RoomCellsEvent onRoomCellsRemoved;

        public delegate void RoomBlockEvent(RoomCells roomBlock);
        public RoomBlockEvent onRoomBlockRemoved;
        public RoomBlockEvent onRoomBlockAdded;

        public delegate void RoomConnectionsEvent(RoomConnections allRoomConnections, RoomConnections roomConnections);
        public RoomConnectionsEvent onRoomConnectionsAdded;
        public RoomConnectionsEvent onRoomConnectionsRemoved;
        public delegate void RoomConnectionsUpdatedEvent(RoomConnections allRoomConnections);
        public RoomConnectionsUpdatedEvent onRoomConnectionsUpdated;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            roomConnections = input.roomConnections ?? new RoomConnections();
            roomList = input.roomList ?? new RoomList();
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
                List<CellCoordinates> perimeterRoomCellCoordinates = newRoom.cells.GetPerimeterCellCoordinates();

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
                    Registry.appState.buildings.buildingList.Add(building);
                }

                newRoom.building = building;
            }

            AddRoom(newRoom);

            if (roomConnections.connections.Count > 0)
            {
                AddRoomConnections(roomConnections);
            }
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

            if (onRoomListUpdated != null)
            {
                onRoomListUpdated(roomList);
            }
        }

        public void DestroyRoom(Room room, bool checkForBuildingDestroy = true)
        {
            if (room == null)
            {
                return;
            }

            roomConnections.RemoveConnectionsForRoom(room);

            if (checkForBuildingDestroy)
            {
                Building building = FindBuildingByRoom(room);

                roomList.Remove(room);

                Building buildingContainingRoom = FindBuildingByRoom(room);

                roomList.Remove(room);

                if (onRoomRemoved != null)
                {
                    onRoomRemoved(room);
                }

                if (onRoomListUpdated != null)
                {
                    onRoomListUpdated(roomList);
                }

                room.OnDestroy();

                RoomList roomsInBuilding = FindRoomsInBuilding(buildingContainingRoom);

                if (roomsInBuilding.Count == 0)
                {
                    Registry.appState.buildings.buildingList.Remove(building);
                }
            }
        }

        // Room Cells
        public void AddRoomCells(Room room, RoomCells cells)
        {
            room.cells.Add(cells);

            if (onRoomCellsAdded != null)
            {
                onRoomCellsAdded(room, cells);
            }
        }

        public void RemoveRoomCells(Room room, RoomCells cells)
        {
            room.cells.Remove(cells);

            if (onRoomCellsRemoved != null)
            {
                onRoomCellsRemoved(room, cells);
            }
        }

        // Room Blocks
        public void DestroyRoomBlock(Room room, RoomCells roomBlock)
        {
            if (room == null)
            {
                return;
            }

            // TODO - check if doing this is going to divide the room into 2
            // if so, create another room right here

            room.RemoveBlock(roomBlock);
            room.ResetRoomCellOrientations();

            // TODO - destroy connections block may have had

            if (room.blocks.blocks.Count == 0)
            {
                DestroyRoom(room);
            }
        }

        // RoomConnections

        public void AddRoomConnections(RoomConnections roomConnections)
        {
            this.roomConnections.Add(roomConnections);

            if (onRoomConnectionsAdded != null)
            {
                onRoomConnectionsAdded(this.roomConnections, roomConnections);
            }

            if (onRoomConnectionsUpdated != null)
            {
                onRoomConnectionsUpdated(this.roomConnections);
            }
        }

        public void RemoveRoomConnections(RoomConnections roomConnections)
        {
            this.roomConnections.Remove(roomConnections);

            if (onRoomConnectionsRemoved != null)
            {
                onRoomConnectionsRemoved(this.roomConnections, roomConnections);
            }

            if (onRoomConnectionsUpdated != null)
            {
                onRoomConnectionsUpdated(this.roomConnections);
            }
        }

        // Queries
        public Building FindBuildingByRoom(Room room)
        {
            foreach (Building building in Registry.appState.buildings.buildingList)
            {
                if (room.building == building)
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
                if (room.building == building)
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
                foreach (int floor in room.cells.GetFloorRange())
                {
                    Room leftRoom = FindRoomAtCell(new CellCoordinates(
                        room.cells.GetLowestX() - 1,
                        floor
                    ));

                    Room rightRoom = FindRoomAtCell(new CellCoordinates(
                        room.cells.GetHighestX() + 1,
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
                foreach (int x in room.cells.GetXRange())
                {
                    Room aboveRoom = FindRoomAtCell(new CellCoordinates(
                        x,
                        room.cells.GetHighestFloor() + 1
                    ));

                    Room belowRoom = FindRoomAtCell(new CellCoordinates(
                        x,
                        room.cells.GetLowestFloor() - 1
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

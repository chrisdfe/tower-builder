using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Buildings;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.Rooms.Validators;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.State.Rooms
{
    [Serializable]
    public partial class State
    {
        public struct Input
        {
            public RoomList roomList;
            public RoomConnections roomConnections;
        }

        public RoomList roomList { get; private set; } = new RoomList();
        public RoomConnections roomConnections { get; private set; } = new RoomConnections();

        public State.Events events;
        public State.Queries queries;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            roomConnections = input.roomConnections ?? new RoomConnections();
            roomList = input.roomList ?? new RoomList();

            events = new State.Events();
            queries = new State.Queries(this);
        }

        /* 
            Rooms
         */
        public void AddRoom(Room room)
        {
            roomList.Add(room);

            if (events.onRoomAdded != null)
            {
                events.onRoomAdded(room);
            }

            if (events.onRoomListUpdated != null)
            {
                events.onRoomListUpdated(roomList);
            }

            room.validator.Validate(Registry.appState);
        }

        public void BuildRoom(Room room)
        {
            room.validator.Validate(Registry.appState);

            if (!room.validator.isValid)
            {
                // TODO - these should be unique messages - right now they are not
                foreach (RoomValidationError validationError in room.validator.errors)
                {
                    Registry.appState.Notifications.AddNotification(validationError.message);
                }
                return;
            }

            // 
            Registry.appState.Wallet.SubtractBalance(room.price);

            // Decide whether to create a new room or to add to an existing one
            List<Room> roomsToCombineWith = queries.FindRoomsToCombineWith(room);


            if (roomsToCombineWith.Count > 0)
            {
                foreach (Room otherRoom in roomsToCombineWith)
                {
                    room.AddBlocks(otherRoom.blocks);
                    DestroyRoom(otherRoom, false);
                }

                room.Reset();
            }

            FindAndAddConnectionsForRoom(room);

            room.isInBlueprintMode = false;
            room.building = FindOrCreateRoomBuilding();
            room.OnBuild();

            if (events.onRoomBuilt != null)
            {
                events.onRoomBuilt(room);
            }

            Building FindOrCreateRoomBuilding()
            {
                List<Room> perimeterRooms = FindPerimeterRooms();

                if (perimeterRooms.Count > 0)
                {
                    // TODO here - if perimeterRooms has more than 1 then combine them
                    return queries.FindBuildingByRoom(perimeterRooms[0]);
                }

                Building building = new Building();
                Registry.appState.buildings.AddBuilding(building);
                return building;
            }

            List<Room> FindPerimeterRooms()
            {
                List<CellCoordinates> perimeterRoomCellCoordinates = room.cells.GetPerimeterCellCoordinates();
                List<Room> result = new List<Room>();

                foreach (CellCoordinates coordinates in perimeterRoomCellCoordinates)
                {
                    Room perimeterRoom = queries.FindRoomAtCell(coordinates);
                    if (perimeterRoom != null)
                    {
                        result.Add(perimeterRoom);
                    }
                }

                return result;
            }
        }


        public void AddAndBuildRoom(Room room)
        {
            AddRoom(room);
            BuildRoom(room);
        }

        public void DestroyRoom(Room room, bool checkForBuildingDestroy = true)
        {
            roomList.Remove(room);

            if (events.onRoomRemoved != null)
            {
                events.onRoomRemoved(room);
            }

            if (events.onRoomListUpdated != null)
            {
                events.onRoomListUpdated(roomList);
            }

            room.OnDestroy();

            RemoveConnectionsForRoom(room);

            if (checkForBuildingDestroy)
            {
                Building buildingContainingRoom = queries.FindBuildingByRoom(room);

                RoomList roomsInBuilding = queries.FindRoomsInBuilding(buildingContainingRoom);

                if (roomsInBuilding.Count == 0)
                {
                    Registry.appState.buildings.RemoveBuilding(buildingContainingRoom);
                }
            }
        }

        /* 
            Room Blocks
         */
        public void AddRoomBlock(Room room, RoomCells roomBlock)
        {
            room.AddBlock(roomBlock);
            room.Reset();

            RemoveConnectionsForRoom(room);
            FindAndAddConnectionsForRoom(room);

            if (events.onRoomBlockAdded != null)
            {
                events.onRoomBlockAdded(room, roomBlock);
            }

            if (events.onRoomBlocksUpdated != null)
            {
                events.onRoomBlocksUpdated(room);
            }
        }

        public void DestroyRoomBlock(Room room, RoomCells roomBlock)
        {
            // TODO - check if doing this is going to divide the room into 2
            // if so, create another room right here

            room.RemoveBlock(roomBlock);

            room.Reset();
            RemoveConnectionsForRoom(room);
            FindAndAddConnectionsForRoom(room);

            if (room.blocks.Count == 0)
            {
                DestroyRoom(room);
            }
            else
            {
                if (events.onRoomBlockRemoved != null)
                {
                    events.onRoomBlockRemoved(room, roomBlock);
                }

                if (events.onRoomBlocksUpdated != null)
                {
                    events.onRoomBlocksUpdated(room);
                }
            }

            /* 
            List<RoomEntrance> GetEntrancesInBlock()
            {
                List<RoomEntrance> result = new List<RoomEntrance>();

                foreach (RoomEntrance entrance in room.entrances)
                {
                    if (roomBlock.Contains(entrance.cellCoordinates))
                    {
                        result.Add(entrance);
                    }
                }

                return result;
            }
            */
        }

        /*
            RoomConnections
        */
        public void FindAndAddConnectionsForRoom(Room room)
        {
            RoomConnections connections = roomConnections.SearchForNewConnectionsToRoom(roomList, room);

            if (connections.Count > 0)
            {
                AddRoomConnections(connections);
            }
        }

        public void AddRoomConnections(RoomConnections newRoomConnections)
        {
            this.roomConnections.Add(newRoomConnections);

            if (events.onRoomConnectionsAdded != null)
            {
                events.onRoomConnectionsAdded(this.roomConnections, newRoomConnections);
            }

            if (events.onRoomConnectionsUpdated != null)
            {
                events.onRoomConnectionsUpdated(this.roomConnections);
            }
        }

        public void RemoveRoomConnection(RoomConnection roomConnection)
        {
            roomConnections.Remove(roomConnection);

            if (events.onRoomConnectionsRemoved != null)
            {
                events.onRoomConnectionsRemoved(this.roomConnections, new RoomConnections(new List<RoomConnection>() { roomConnection }));
            }

            if (events.onRoomConnectionsUpdated != null)
            {
                events.onRoomConnectionsUpdated(this.roomConnections);
            }
        }

        public void RemoveConnectionsForRoom(Room room)
        {
            RoomConnections roomConnectionsForRoom = this.roomConnections.FindConnectionsForRoom(room);

            if (roomConnectionsForRoom.Count == 0) return;

            roomConnections.Remove(roomConnectionsForRoom);

            if (events.onRoomConnectionsRemoved != null)
            {
                events.onRoomConnectionsRemoved(this.roomConnections, roomConnectionsForRoom);
            }

            if (events.onRoomConnectionsUpdated != null)
            {
                events.onRoomConnectionsUpdated(this.roomConnections);
            }
        }

        public void RemoveRoomConnections(RoomConnections roomConnections)
        {
            this.roomConnections.Remove(roomConnections);

            if (events.onRoomConnectionsRemoved != null)
            {
                events.onRoomConnectionsRemoved(this.roomConnections, roomConnections);
            }

            if (events.onRoomConnectionsUpdated != null)
            {
                events.onRoomConnectionsUpdated(this.roomConnections);
            }
        }
    }
}

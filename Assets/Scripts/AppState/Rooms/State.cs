using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.Rooms.Blueprints;
using TowerBuilder.State.Rooms.Connections;
using TowerBuilder.State.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.State.Rooms
{
    [Serializable]
    public class State
    {
        public struct Input
        {
            public RoomList rooms;
            public BuildingList buildings;
            public RoomConnections roomConnections;
        }

        public BuildingList buildings { get; private set; } = new BuildingList();

        public RoomList rooms { get; private set; } = new RoomList();

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
            rooms = input.rooms ?? new RoomList();
            roomConnections = input.roomConnections ?? new RoomConnections();
        }

        public void AddRoom(Room room)
        {
            if (room == null)
            {
                return;
            }

            rooms.Add(room);

            room.OnBuild();

            if (onRoomAdded != null)
            {
                onRoomAdded(room);
            }
        }

        public void AttemptToAddRoom(Blueprint blueprint, RoomConnections roomConnections)
        {
            // TODO - should everything here on down go in MapState?
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
            if (roomsToCombineWith.Count > 0)
            {
                foreach (Room otherRoom in roomsToCombineWith)
                {
                    newRoom.AddBlocks(otherRoom.blocks);

                    DestroyRoom(otherRoom);
                }

                // TODO - add to the 1st item in roomsToCombineWith instead of replacing both with a new room?

                // TODO - this might not be the best place to call this
                newRoom.Reset();
            }

            AddRoom(newRoom);

            if (roomConnections.connections.Count > 0)
            {
                AddRoomConnections(roomConnections);
            }
        }


        public void AddRoomCell(RoomCell roomCell)
        {

        }

        public void DestroyRoom(Room room)
        {
            if (room == null)
            {
                return;
            }

            RemoveRoomConnectionsForRoom(room);

            rooms.Remove(room);

            if (onRoomDestroyed != null)
            {
                onRoomDestroyed(room);
            }
        }

        public void DestroyRoomBlock(Room room, RoomCells roomBlock)
        {
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
                    Room leftRoom = rooms.FindRoomAtCell(new CellCoordinates(
                        room.roomCells.GetLowestX() - 1,
                        floor
                    ));

                    Room rightRoom = rooms.FindRoomAtCell(new CellCoordinates(
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
                    Room aboveRoom = Registry.appState.Rooms.rooms.FindRoomAtCell(new CellCoordinates(
                        x,
                        room.roomCells.GetHighestFloor() + 1
                    ));

                    Room belowRoom = Registry.appState.Rooms.rooms.FindRoomAtCell(new CellCoordinates(
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

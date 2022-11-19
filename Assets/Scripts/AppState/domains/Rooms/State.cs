using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Connections;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Rooms
{
    [Serializable]
    public partial class State : StateSlice
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

        public State(AppState appState, Input input) : base(appState)
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

            room.validator.Validate(appState);

            if (room.validator.isValid)
            {
                FindAndAddConnectionsForRoom(room);
            }
        }

        public void BuildRoom(Room room)
        {
            room.validator.Validate(appState);

            if (!room.validator.isValid)
            {
                // TODO - these should be unique messages - right now they are not
                foreach (RoomValidationError validationError in room.validator.errors)
                {
                    appState.Notifications.AddNotification(validationError.message);
                }
                return;
            }

            // 
            appState.Wallet.SubtractBalance(room.price);

            // Decide whether to create a new room or to add to an existing one
            List<Room> roomsToCombineWith = queries.FindRoomsToCombineWith(room);

            if (roomsToCombineWith.Count > 0)
            {
                foreach (Room otherRoom in roomsToCombineWith)
                {
                    room.blocks.Add(otherRoom.blocks);
                    DestroyRoom(otherRoom);
                }

                room.Reset();
            }

            room.isInBlueprintMode = false;
            room.OnBuild();

            if (events.onRoomBuilt != null)
            {
                events.onRoomBuilt(room);
            }
        }

        public void AddAndBuildRoom(Room room)
        {
            AddRoom(room);
            BuildRoom(room);
        }

        public void DestroyRoom(Room room)
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
        }

        /* 
            Room Blocks
         */
        public void AddRoomBlock(Room room, RoomCells roomBlock)
        {
            room.blocks.Add(roomBlock);
            room.Reset();

            RemoveConnectionsForRoom(room);
            FindAndAddConnectionsForRoom(room);

            if (events.onRoomBlocksAdded != null)
            {
                events.onRoomBlocksAdded(room, new RoomBlocks(roomBlock));
            }

            if (events.onRoomBlocksUpdated != null)
            {
                events.onRoomBlocksUpdated(room);
            }
        }

        public void DestroyRoomBlocks(Room room, RoomBlocks roomBlocks)
        {
            // TODO - check if doing this is going to divide the room into 2
            // if so, create another room right here

            foreach (RoomCells block in roomBlocks.blocks)
            {
                room.blocks.Remove(block);
            }

            if (room.blocks.Count == 0)
            {
                DestroyRoom(room);
            }
            else
            {
                room.Reset();
                RemoveConnectionsForRoom(room);
                FindAndAddConnectionsForRoom(room);

                if (events.onRoomBlocksRemoved != null)
                {
                    events.onRoomBlocksRemoved(room, roomBlocks);
                }

                if (events.onRoomBlocksUpdated != null)
                {
                    events.onRoomBlocksUpdated(room);
                }
            }
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

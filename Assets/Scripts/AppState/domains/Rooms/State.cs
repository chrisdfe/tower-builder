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
            public RoomConnectionList roomConnectionList;
        }

        public RoomList roomList { get; private set; } = new RoomList();
        public RoomConnectionList roomConnectionList { get; private set; } = new RoomConnectionList();

        public Events events;
        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            roomConnectionList = input.roomConnectionList ?? new RoomConnectionList();
            roomList = input.roomList ?? new RoomList();

            events = new Events();
            queries = new Queries(this);
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
            RoomConnectionList connections = roomConnectionList.SearchForNewConnectionsToRoom(roomList, room);

            if (connections.Count > 0)
            {
                AddRoomConnections(connections);
            }
        }

        public void AddRoomConnections(RoomConnectionList newRoomConnections)
        {
            this.roomConnectionList.Add(newRoomConnections);

            if (events.onRoomConnectionsAdded != null)
            {
                events.onRoomConnectionsAdded(this.roomConnectionList, newRoomConnections);
            }

            if (events.onRoomConnectionListUpdated != null)
            {
                events.onRoomConnectionListUpdated(this.roomConnectionList);
            }
        }

        public void RemoveRoomConnection(RoomConnection roomConnection)
        {
            roomConnectionList.Remove(roomConnection);

            if (events.onRoomConnectionsRemoved != null)
            {
                events.onRoomConnectionsRemoved(this.roomConnectionList, new RoomConnectionList(new List<RoomConnection>() { roomConnection }));
            }

            if (events.onRoomConnectionListUpdated != null)
            {
                events.onRoomConnectionListUpdated(this.roomConnectionList);
            }
        }

        public void RemoveConnectionsForRoom(Room room)
        {
            RoomConnectionList roomConnectionsForRoom = this.roomConnectionList.FindConnectionsForRoom(room);

            if (roomConnectionsForRoom.Count == 0) return;

            roomConnectionList.Remove(roomConnectionsForRoom);

            if (events.onRoomConnectionsRemoved != null)
            {
                events.onRoomConnectionsRemoved(this.roomConnectionList, roomConnectionsForRoom);
            }

            if (events.onRoomConnectionListUpdated != null)
            {
                events.onRoomConnectionListUpdated(this.roomConnectionList);
            }
        }

        public void RemoveRoomConnections(RoomConnectionList roomConnections)
        {
            this.roomConnectionList.Remove(roomConnections);

            if (events.onRoomConnectionsRemoved != null)
            {
                events.onRoomConnectionsRemoved(this.roomConnectionList, roomConnections);
            }

            if (events.onRoomConnectionListUpdated != null)
            {
                events.onRoomConnectionListUpdated(this.roomConnectionList);
            }
        }
    }
}

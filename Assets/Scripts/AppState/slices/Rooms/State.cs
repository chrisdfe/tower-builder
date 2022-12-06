using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Rooms
{
    using RoomsListStateSlice = ListStateSlice<RoomList, Room, State.Events>;

    [Serializable]
    public partial class State : RoomsListStateSlice
    {
        public struct Input
        {
            public RoomList roomList;
        }

        public new class Events : RoomsListStateSlice.Events
        {
            public RoomsListStateSlice.Events.ItemEvent onItemBuilt;

            public delegate void RoomBlocksEvent(Room room, RoomBlocks roomBlocks);
            public RoomBlocksEvent onRoomBlocksAdded;
            public RoomBlocksEvent onRoomBlocksRemoved;

            public delegate void RoomBlockUpdatedEvent(Room room);
            public RoomBlocksEvent onRoomBlocksUpdated;
        }

        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.roomList ?? new RoomList();

            queries = new Queries(this);
        }

        /* 
            Rooms
        */
        public override void Add(Room room)
        {
            base.Add(room);
            room.validator.Validate(appState);
        }

        public void Build(Room room)
        {
            room.validator.Validate(appState);

            if (!room.validator.isValid)
            {
                // TODO - these should be unique messages - right now they are not
                foreach (RoomValidationError validationError in room.validator.errors)
                {
                    appState.Notifications.Add(new Notification(validationError.message));
                }
                return;
            }

            // 
            appState.Wallet.SubtractBalance(room.price);

            /*
            // Decide whether to create a new room or to add to an existing one
            List<Room> roomsToCombineWith = queries.FindRoomsToCombineWith(room);

            if (roomsToCombineWith.Count > 0)
            {
                foreach (Room otherRoom in roomsToCombineWith)
                {
                    room.blocks.Add(otherRoom.blocks);
                    Remove(otherRoom);
                }

                room.Reset();
            }
            */

            room.isInBlueprintMode = false;
            room.OnBuild();

            events.onItemBuilt?.Invoke(room);
        }

        public override void Remove(Room room)
        {
            room.OnDestroy();
            base.Remove(room);
        }

        /* 
            Room Blocks
         */
        public void AddRoomBlock(Room room, RoomCells roomBlock)
        {
            room.blocks.Add(roomBlock);
            room.Reset();

            events.onRoomBlocksAdded?.Invoke(room, new RoomBlocks(roomBlock));
            events.onRoomBlocksUpdated?.Invoke(room, room.blocks);
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
                Remove(room);
            }
            else
            {
                room.Reset();

                events.onRoomBlocksRemoved?.Invoke(room, roomBlocks);
                events.onRoomBlocksUpdated?.Invoke(room, room.blocks);
            }
        }
    }
}

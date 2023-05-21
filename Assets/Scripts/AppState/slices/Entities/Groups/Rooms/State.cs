using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using TowerBuilder.DataTypes.EntityGroups.Rooms.Validators;
using TowerBuilder.DataTypes.Notifications;
using UnityEngine;

namespace TowerBuilder.ApplicationState.Entities.Groups.Rooms
{
    [Serializable]
    public class State : EntityGroupStateSlice<Room, State.Events>
    {
        public struct Input
        {
            public List<Room> roomList;
        }

        public new class Events : EntityGroupStateSlice<Room, State.Events>.Events
        {
            public delegate void RoomBlocksEvent(Room room, CellCoordinatesBlockList roomBlocks);
            public RoomBlocksEvent onRoomBlocksAdded { get; set; }
            public RoomBlocksEvent onRoomBlocksRemoved { get; set; }

            public delegate void RoomBlockUpdatedEvent(Room room);
            public RoomBlocksEvent onRoomBlocksUpdated { get; set; }
        }

        public new Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            queries = new Queries(appState, this);
        }

        /* 
            Rooms
        */
        protected void OnPreBuild(Room room)
        {
            // ListWrapper<Room> overlappingRooms = list.FindAll((otherRoom) =>
            //     otherRoom != room &&
            //     otherRoom.cellCoordinatesList.OverlapsWith(room.cellCoordinatesList)
            // );

            // // Merge Rooms together
            // if (overlappingRooms.Count > 0)
            // {
            //     Room newRoom = new Room(room.definition as RoomDefinition);

            //     foreach (Room otherRoom in overlappingRooms.items)
            //     {
            //         room.cellCoordinatesList.Add(otherRoom.cellCoordinatesList);
            //         Remove(otherRoom);
            //     }

            //     newRoom.CalculateTileableMap();
            // }
        }

        /* 
            Room Blocks
         */
        public void AddRoomBlocks(Room room, CellCoordinatesBlockList roomBlockList)
        {
            // room.blocksList.Add(roomBlockList);
            // room.cellCoordinatesList.Add(roomBlockList.Flatten());

            // // TODO here - recalculate occupied cell maps etc

            // events.onRoomBlocksAdded?.Invoke(room, roomBlockList);
            // events.onRoomBlocksUpdated?.Invoke(room, room.blocksList);
        }

        public void DestroyRoomBlocks(Room room, CellCoordinatesBlockList roomBlockList)
        {
            // TODO - check if doing this is going to divide the room into 2
            // if so, create another room right here

            // room.blocksList.Remove(roomBlockList);
            // room.cellCoordinatesList.Remove(roomBlockList.Flatten());

            // if (room.blocksList.Count == 0)
            // {
            //     Remove(room);
            // }
            // else
            // {
            //     room.CalculateTileableMap();

            //     events.onRoomBlocksRemoved?.Invoke(room, roomBlockList);
            //     events.onRoomBlocksUpdated?.Invoke(room, room.blocksList);
            // }
        }

        public new class Queries : EntityGroupStateSlice<Room, State.Events>.Queries
        {
            public Queries(AppState appState, State state) : base(appState, state) { }

            // public Room FindRoomAtCell(CellCoordinates cellCoordinates) =>
            //     state.list.items
            //         .Find(room => room.cellCoordinatesList.Contains(cellCoordinates));

            // public (Room, CellCoordinatesBlock) FindRoomBlockAtCell(CellCoordinates cellCoordinates)
            // {
            //     Room room = FindRoomAtCell(cellCoordinates);

            //     if (room != null)
            //     {
            //         CellCoordinatesBlock roomBlock = room.FindBlockByCellCoordinates(cellCoordinates);

            //         if (roomBlock != null)
            //         {
            //             return (room, roomBlock);
            //         }
            //     }

            //     return (null, null);
            // }

            // public List<Room> FindPerimeterRooms(Room room)
            // {
            //     List<CellCoordinates> perimeterRoomCellCoordinates = room.cellCoordinatesList.GetPerimeterCellCoordinates();
            //     List<Room> result = new List<Room>();

            //     foreach (CellCoordinates coordinates in perimeterRoomCellCoordinates)
            //     {
            //         Room perimeterRoom = FindRoomAtCell(coordinates);
            //         if (perimeterRoom != null && perimeterRoom != room)
            //         {
            //             result.Add(perimeterRoom);
            //         }
            //     }

            //     return result;
            // }
        }
    }
}

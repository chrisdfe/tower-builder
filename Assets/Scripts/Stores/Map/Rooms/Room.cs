using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms.Modules;
using TowerBuilder.Stores.Map.Rooms.Uses;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class Room
    {
        public string id { get; private set; }
        public RoomKey roomKey { get; private set; }

        public RoomCells roomCells;
        public List<RoomEntrance> roomEntrances { get; private set; }
        public List<RoomModuleBase> modules { get; private set; }

        public RoomDetails roomDetails
        {
            get
            {
                return Rooms.Constants.ROOM_DETAILS_MAP[roomKey];
            }
        }

        public Room(RoomKey roomKey)
        {
            id = GenerateId();
            this.roomKey = roomKey;
            roomCells = new RoomCells(this);
        }

        public Room(RoomKey roomKey, RoomCells roomCells)
        {
            id = GenerateId();
            this.roomKey = roomKey;
            SetRoomCells(roomCells);
        }

        public void OnBuild()
        {
            InitializeRoomEntrances();
            InitializeModules();
        }

        public void SetRoomKey(RoomKey roomKey)
        {
            this.roomKey = roomKey;
        }

        public void SetRoomCells(RoomCells roomCells)
        {
            this.roomCells = roomCells;
            roomCells.SetRoom(this);
        }

        public void SetEntrances(List<RoomEntrance> roomEntrances)
        {
            this.roomEntrances = roomEntrances;
            InitializeRoomEntrances();
        }

        string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        void InitializeRoomEntrances()
        {
            roomEntrances = new List<RoomEntrance>(roomDetails.entrances);

            foreach (RoomEntrance roomEntrance in roomEntrances)
            {
                // RoomEntrances need a Room instance attached
                roomEntrance.room = this;

                foreach (RoomCell roomCell in roomCells.cells)
                {
                    if (roomCell.coordinates.Matches(roomEntrance.cellCoordinates))
                    {
                        roomCell.AddEntrance(roomEntrance);
                    }
                }
            }
        }

        void InitializeModules()
        {
            modules = new List<RoomModuleBase>();

            // TODO - there's probably a more elegant way of doing this that I will figure out later
            foreach (RoomUseDetailsBase roomUseDetails in roomDetails.useDetails)
            {
                switch (roomUseDetails.roomUseKey)
                {
                    case RoomUseKey.Elevator:
                        ElevatorModule elevatorModule = new ElevatorModule(this);
                        modules.Add(elevatorModule);
                        break;
                }
            }

            foreach (RoomModuleBase roomModule in modules)
            {
                roomModule.Initialize();
            }
        }
    }
}



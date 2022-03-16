using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms.EntranceBuilders;
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
        public List<RoomModuleBase> modules { get; private set; } = new List<RoomModuleBase>();
        public List<RoomEntrance> entrances { get; private set; } = new List<RoomEntrance>();

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

        public Room(RoomKey roomKey, List<RoomCell> roomCellList) : this(roomKey)
        {
            roomCells.Add(roomCellList);
            ResetRoomEntrances();
        }

        public Room(RoomKey roomKey, RoomCells roomCells) : this(roomKey)
        {
            roomCells.Add(roomCells);
            ResetRoomEntrances();
        }

        public void OnBuild()
        {
            Debug.Log("OnBuild");
            foreach (RoomCell roomCell in roomCells.cells)
            {
                Debug.Log(roomCell.relativeCellCoordinates);
            }
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
            ResetRoomEntrances();
        }

        string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        void ResetRoomEntrances()
        {
            List<RoomEntrance> result = new List<RoomEntrance>();

            if (roomDetails.entrances.Count > 0)
            {
                result = roomDetails.entrances;
            }
            else
            {
                if (roomDetails.category == RoomCategory.Elevator)
                {
                    result = ElevatorEntranceBuilder.BuildRoomEntrances(roomCells);
                }
            }

            entrances = result;
        }

        void InitializeModules()
        {
            List<RoomModuleBase> result = new List<RoomModuleBase>();

            // TODO - there's probably a more elegant way of doing this that I will figure out later
            foreach (RoomUseDetailsBase roomUseDetails in roomDetails.useDetails)
            {
                switch (roomUseDetails.roomUseKey)
                {
                    case RoomUseKey.Elevator:
                        ElevatorModule elevatorModule = new ElevatorModule(this);
                        result.Add(elevatorModule);
                        break;
                }
            }

            foreach (RoomModuleBase roomModule in result)
            {
                roomModule.Initialize();
            }

            modules = result;
        }
    }
}



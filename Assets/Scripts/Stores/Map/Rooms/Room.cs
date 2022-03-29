using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TowerBuilder.Stores.Map.Rooms.EntranceBuilders;
using TowerBuilder.Stores.Map.Rooms.Furniture;
using TowerBuilder.Stores.Map.Rooms.Modules;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class Room
    {
        private static int autoincrementingId;
        public int id { get; private set; }
        public RoomKey roomKey { get; private set; }

        public bool isInBlueprintMode = false;

        public RoomCells roomCells;
        public List<RoomModuleBase> modules { get; private set; } = new List<RoomModuleBase>();
        public List<RoomEntrance> entrances { get; private set; } = new List<RoomEntrance>();
        public List<RoomFurnitureBase> furniture { get; private set; } = new List<RoomFurnitureBase>();

        public RoomDetails roomDetails { get; private set; }

        public Room()
        {
            GenerateId();
            roomDetails = new RoomDetails();
            roomCells = new RoomCells();
            roomCells.onResize += OnRoomCellsResize;
        }

        public Room(RoomKey roomKey) : this()
        {
            SetRoomKey(roomKey);
        }

        public Room(RoomKey roomKey, List<RoomCell> roomCellList) : this(roomKey)
        {
            roomCells.Add(roomCellList);
            ResetRoomCellOrientations();
            ResetRoomEntrances();
        }

        public Room(RoomKey roomKey, RoomCells roomCells) : this(roomKey, roomCells.cells) { }

        public Room(RoomDetails roomDetails) : this()
        {
            this.roomDetails = roomDetails;
        }

        public override string ToString()
        {
            return $"room {id}";
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
            // InitializeModules();
            InitializeFurniture();
        }

        public void SetRoomKey(RoomKey roomKey)
        {
            this.roomKey = roomKey;
            this.roomDetails = Rooms.Constants.ROOM_DETAILS_MAP[roomKey];
        }

        public void SetRoomCells(RoomCells roomCells)
        {
            this.roomCells.Set(roomCells);
            ResetRoomCellOrientations();
            ResetRoomEntrances();
        }

        void GenerateId()
        {
            id = Interlocked.Increment(ref autoincrementingId);
        }

        void OnRoomCellsResize(RoomCells roomCells)
        {
            ResetRoomCellOrientations();
            ResetRoomEntrances();
        }

        void ResetRoomEntrances()
        {
            if (roomDetails.entranceBuilder != null)
            {
                entrances = roomDetails.entranceBuilder.BuildRoomEntrances(roomCells);
            }
        }

        void ResetRoomCellOrientations()
        {
            foreach (RoomCell roomCell in roomCells.cells)
            {
                SetRoomCellOrientation(roomCell);
            }
        }

        void SetRoomCellOrientation(RoomCell roomCell)
        {
            CellCoordinates coordinates = roomCell.coordinates;

            List<RoomCellOrientation> result = new List<RoomCellOrientation>();

            if (!roomCells.HasCellAtCoordinates(new CellCoordinates(coordinates.x, coordinates.floor + 1)))
            {
                result.Add(RoomCellOrientation.Top);
            }

            if (!roomCells.HasCellAtCoordinates(new CellCoordinates(coordinates.x + 1, coordinates.floor)))
            {
                result.Add(RoomCellOrientation.Right);
            }

            if (!roomCells.HasCellAtCoordinates(new CellCoordinates(coordinates.x, coordinates.floor - 1)))
            {
                result.Add(RoomCellOrientation.Bottom);
            }

            if (!roomCells.HasCellAtCoordinates(new CellCoordinates(coordinates.x - 1, coordinates.floor)))
            {
                result.Add(RoomCellOrientation.Left);
            }

            roomCell.orientation = result;
        }

        /* 
        void InitializeModules()
        {
            List<RoomModuleBase> result = new List<RoomModuleBase>();

            // TODO - there's probably a more elegant way of doing this that I will figure out later
            foreach (RoomModuleDetailsBase roomUseDetails in roomDetails.moduleDetails)
            {
                switch (roomUseDetails.roomModuleKey)
                {
                    case RoomModuleKey.Elevator:
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
        */

        void InitializeFurniture() { }

        CellCoordinates GetRelativeCoordinates(RoomCell roomCell)
        {
            return roomCell.coordinates.Subtract(new CellCoordinates(
                roomCells.GetLowestX(),
                roomCells.GetLowestFloor()
            ));
        }
    }
}



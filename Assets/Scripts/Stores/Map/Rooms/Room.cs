using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TowerBuilder.Stores.Map.Rooms.EntranceBuilders;
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

        public RoomDetails roomDetails
        {
            get
            {
                return Rooms.Constants.ROOM_DETAILS_MAP[roomKey];
            }
        }

        public Room(RoomKey roomKey)
        {
            GenerateId();
            this.roomKey = roomKey;

            roomCells = new RoomCells();
            roomCells.onResize += OnRoomCellsResize;
        }

        public Room(RoomKey roomKey, List<RoomCell> roomCellList) : this(roomKey)
        {
            roomCells.Add(roomCellList);
            ResetRoomCells();
        }

        public Room(RoomKey roomKey, RoomCells roomCells) : this(roomKey)
        {
            roomCells.Add(roomCells);
            ResetRoomCells();
        }

        public override string ToString()
        {
            return $"room {id}";
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
            InitializeModules();
        }

        public void SetRoomKey(RoomKey roomKey)
        {
            this.roomKey = roomKey;
        }

        public void SetRoomCells(RoomCells roomCells)
        {
            this.roomCells.Set(roomCells);
            ResetRoomCells();
        }

        void OnRoomCellsResize(RoomCells roomCells)
        {
            ResetRoomCells();
        }

        void GenerateId()
        {
            id = Interlocked.Increment(ref autoincrementingId);
        }

        void ResetRoomCells()
        {
            ResetRoomCellPositions();
            ResetRoomEntrances();
        }

        void ResetRoomEntrances()
        {
            entrances = new List<RoomEntrance>();

            if (roomDetails.entrances.Count > 0)
            {
                entrances = roomDetails.entrances.Select(roomEntrance =>
                {
                    // Convert cellCoordinates from relative to absolute
                    RoomEntrance clonedRoomEntrance = roomEntrance.Clone();
                    clonedRoomEntrance.cellCoordinates = roomEntrance.cellCoordinates.Add(roomCells.GetBottomLeftCoordinates());
                    return clonedRoomEntrance;
                }).ToList();
            }
            else
            {
                // Rooms with flexible sizes use EntranceBuilders 
                switch (roomDetails.category)
                {
                    case RoomCategory.Elevator:
                        entrances = ElevatorEntranceBuilder.BuildRoomEntrances(roomCells);
                        break;
                    case RoomCategory.Lobby:
                        entrances = LobbyEntranceBuilder.BuildRoomEntrances(roomCells);
                        break;
                    case RoomCategory.Hallway:
                        entrances = HallwayEntranceBuilder.BuildRoomEntrances(roomCells);
                        break;
                    case RoomCategory.Stairs:
                        entrances = StairwellEntranceBuilder.BuildRoomEntrances(roomCells);
                        break;
                }
            }
        }

        void ResetRoomCellPositions()
        {
            foreach (RoomCell roomCell in roomCells.cells)
            {
                SetRoomCellPosition(roomCell);
            }
        }

        void SetRoomCellPosition(RoomCell roomCell)
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

        CellCoordinates GetRelativeCoordinates(RoomCell roomCell)
        {
            return roomCell.coordinates.Subtract(new CellCoordinates(
                roomCells.GetLowestX(),
                roomCells.GetLowestFloor()
            ));
        }
    }
}



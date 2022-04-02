using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TowerBuilder.Stores.Rooms.Entrances;
using TowerBuilder.Stores.Rooms.Furniture;

namespace TowerBuilder.Stores.Rooms
{
    public class Room
    {
        private static int autoincrementingId;
        public int id { get; private set; }

        // public RoomKey roomKey { get; private set; }
        public string roomKey { get; private set; } = "";

        public bool isInBlueprintMode = false;

        public RoomCells roomCells;

        public List<RoomEntrance> entrances { get; private set; } = new List<RoomEntrance>();
        public List<RoomFurnitureBase> furniture { get; private set; } = new List<RoomFurnitureBase>();

        public RoomTemplate roomTemplate { get; private set; }

        public Room()
        {
            GenerateId();
            roomTemplate = new RoomTemplate();
            roomCells = new RoomCells();
            roomCells.onResize += OnRoomCellsResize;
        }

        public Room(RoomTemplate roomTemplate) : this()
        {
            this.roomTemplate = roomTemplate;
        }

        public Room(RoomTemplate roomTemplate, List<RoomCell> roomCellList) : this(roomTemplate)
        {
            roomCells.Add(roomCellList);
            ResetRoomCellOrientations();
            ResetRoomEntrances();
        }

        public Room(RoomTemplate roomTemplate, RoomCells roomCells) : this(roomTemplate, roomCells.cells) { }

        public override string ToString()
        {
            return $"room {id}";
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;

            InitializeFurniture();
        }

        public void SetTemplate(RoomTemplate roomTemplate)
        {
            this.roomTemplate = roomTemplate;
        }

        public void SetRoomCells(RoomCells roomCells)
        {
            this.roomCells.Set(roomCells);
            ResetRoomCellOrientations();
            ResetRoomEntrances();
        }

        public int GetPrice()
        {
            // Subtract appropriate balance from wallet
            if (roomTemplate.resizability.Matches(RoomResizability.Inflexible()))
            {
                return roomTemplate.price;
            }

            CellCoordinates copies = GetCopies();
            // Work out how many copies of the base blueprint size is being built
            // roomTemplate.price is per base blueprint copy
            // TODO - this calculation should be elsewhere to allow this number to show up in the UI
            //        as the blueprint is being built

            int result = roomTemplate.price * copies.x * copies.floor;

            return result;
        }

        public CellCoordinates GetCopies()
        {
            return new CellCoordinates(
                roomCells.GetWidth() / roomTemplate.width,
                roomCells.GetFloorSpan() / roomTemplate.height
            );
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
            if (roomTemplate.entranceBuilderFactory != null)
            {
                RoomEntranceBuilderBase entranceBuilder = roomTemplate.entranceBuilderFactory();
                entrances = entranceBuilder.BuildRoomEntrances(roomCells);
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
            foreach (RoomModuleDetailsBase roomUseDetails in roomTemplate.moduleDetails)
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



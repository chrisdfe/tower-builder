using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TowerBuilder.DataTypes.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    [Serializable]
    public class Room
    {
        private static int autoincrementingId;

        public int id { get; private set; }

        public string roomKey { get; private set; } = "";

        [NonSerialized]
        public bool isInBlueprintMode = false;

        [NonSerialized]
        public CellCoordinates bottomLeftCoordinates;

        [NonSerialized]
        public CellCoordinates blockCount;

        public List<RoomCells> blocks;

        public RoomCells roomCells;

        public List<RoomEntrance> entrances { get; private set; } = new List<RoomEntrance>();
        // public List<RoomFurnitureBase> furniture { get; private set; } = new List<RoomFurnitureBase>();

        [SerializeField]
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

        public void CalculateRoomCells()
        {
            roomCells = new RoomCells();
            blocks = new List<RoomCells>();

            Dimensions blockDimensions = this.roomTemplate.blockDimensions;
            for (int x = 0; x < blockCount.x; x++)
            {
                for (int floor = 0; floor < blockCount.floor; floor++)
                {
                    RoomCells blockCells = new RoomCells(
                        blockDimensions.width,
                        blockDimensions.height
                    );
                    blockCells.PositionAtCoordinates(new CellCoordinates(
                        bottomLeftCoordinates.x + (blockDimensions.width * x),
                        bottomLeftCoordinates.floor + (blockDimensions.height * floor)
                    ));
                    blocks.Add(blockCells);
                    roomCells.Add(blockCells);
                }
            }

            Reset();
        }

        public void Reset()
        {
            ResetRoomCellOrientations();
            ResetRoomEntrances();
        }

        public bool ContainsBlock(RoomCells roomBlock)
        {
            foreach (RoomCells block in blocks)
            {
                if (block == roomBlock)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddBlocks(List<RoomCells> blocks)
        {
            foreach (RoomCells block in blocks)
            {
                this.blocks.Add(block);
                roomCells.Add(block);
            }
        }

        public void RemoveBlock(RoomCells block)
        {
            foreach (RoomCell roomCell in block.cells)
            {
                roomCells.cells.Remove(roomCell);
            }

            this.blocks.Remove(block);
        }

        public int GetPrice()
        {
            return roomTemplate.price * this.blocks.Count;
        }

        public RoomCells FindBlockByCellCoordinates(CellCoordinates cellCoordinates)
        {
            foreach (RoomCells block in blocks)
            {
                if (block.Contains(cellCoordinates))
                {
                    return block;
                }
            }

            return null;
        }

        public void ResetRoomCellOrientations()
        {
            foreach (RoomCell roomCell in roomCells.cells)
            {
                SetRoomCellOrientation(roomCell);
            }
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

        void SetRoomCellOrientation(RoomCell roomCell)
        {
            CellCoordinates coordinates = roomCell.coordinates;

            List<RoomCellOrientation> result = new List<RoomCellOrientation>();

            if (!roomCells.Contains(new CellCoordinates(coordinates.x, coordinates.floor + 1)))
            {
                result.Add(RoomCellOrientation.Top);
            }

            if (!roomCells.Contains(new CellCoordinates(coordinates.x + 1, coordinates.floor)))
            {
                result.Add(RoomCellOrientation.Right);
            }

            if (!roomCells.Contains(new CellCoordinates(coordinates.x, coordinates.floor - 1)))
            {
                result.Add(RoomCellOrientation.Bottom);
            }

            if (!roomCells.Contains(new CellCoordinates(coordinates.x - 1, coordinates.floor)))
            {
                result.Add(RoomCellOrientation.Left);
            }

            roomCell.orientation = result;
        }

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



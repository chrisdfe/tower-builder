using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class Room
    {
        private static int autoincrementingId;

        public int id { get; private set; }

        public string title { get; private set; } = "None";

        public string key { get; private set; } = "None";

        public string category { get; private set; } = "None";

        public int pricePerBlock { get; private set; } = 10;

        public RoomResizability resizability = RoomResizability.Inflexible();

        public Dimensions blockDimensions { get; private set; } = Dimensions.one;

        // TODO - this is only JsonIgnore because it's a recursive type and
        //        the serializer doesn't like that.
        //        fix this instead of ignoring it
        [JsonIgnore]
        public Color color { get; private set; } = Color.white;


        // Saved rooms should never be in blueprint mode
        [JsonIgnore]
        public bool isInBlueprintMode = false;

        public CellCoordinates bottomLeftCoordinates;

        public List<RoomCells> blocks;

        public RoomCells roomCells;

        public List<RoomEntrance> entrances { get; private set; } = new List<RoomEntrance>();

        public RoomValidatorBase validator { get; private set; }
        public RoomEntranceBuilderBase entranceBuilder { get; private set; }

        [JsonIgnore]
        public int price { get { return pricePerBlock * this.blocks.Count; } }

        public Room(RoomTemplate roomTemplate)
        {
            GenerateId();
            // this.roomTemplate = roomTemplate;

            this.title = roomTemplate.title;
            this.key = roomTemplate.key;
            this.category = roomTemplate.category;
            this.pricePerBlock = roomTemplate.pricePerBlock;
            this.resizability = roomTemplate.resizability;
            this.validator = roomTemplate.validatorFactory();
            this.entranceBuilder = roomTemplate.entranceBuilderFactory();

            this.color = roomTemplate.color;

            roomCells = new RoomCells();
            roomCells.onResize += OnRoomCellsResize;
        }

        // public Room() : this(new RoomTemplate()) { }

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

        public void CalculateRoomCells(CellCoordinates blockCount)
        {
            roomCells = new RoomCells();
            blocks = new List<RoomCells>();

            Dimensions blockDimensions = this.blockDimensions;
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
            entrances = entranceBuilder.BuildRoomEntrances(roomCells);
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



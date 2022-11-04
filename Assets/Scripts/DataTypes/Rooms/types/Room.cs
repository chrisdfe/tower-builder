using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using TowerBuilder.DataTypes.Buildings;
using TowerBuilder.DataTypes.Rooms.Entrances;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class Room
    {
        public int id { get; private set; }

        public string title { get; private set; } = "None";
        public string key { get; private set; } = "None";
        public string category { get; private set; } = "None";

        public int pricePerBlock { get; private set; } = 10;

        public RoomResizability resizability = RoomResizability.Inflexible();

        public Dimensions blockDimensions { get; private set; } = Dimensions.one;

        public Building building;

        // TODO - this is only JsonIgnore because it's a recursive type and
        //        the serializer doesn't like that.
        //        fix this instead of ignoring it
        [JsonIgnore]
        public Color color { get; private set; } = Color.white;

        // Saved rooms should never be in blueprint mode
        [JsonIgnore]
        public bool isInBlueprintMode = false;

        public CellCoordinates bottomLeftCoordinates;

        public RoomBlocks blocks;
        public RoomCells cells;

        public List<RoomEntrance> entrances { get; private set; } = new List<RoomEntrance>();

        public RoomValidatorBase validator { get; private set; }
        public RoomEntranceBuilderBase entranceBuilder { get; private set; }

        [JsonIgnore]
        public int price { get { return pricePerBlock * this.blocks.blocks.Count; } }

        public Room(RoomTemplate roomTemplate)
        {
            this.id = UIDGenerator.Generate("room");
            this.title = roomTemplate.title;
            this.key = roomTemplate.key;
            this.category = roomTemplate.category;
            this.pricePerBlock = roomTemplate.pricePerBlock;
            this.resizability = roomTemplate.resizability;
            this.blockDimensions = roomTemplate.blockDimensions;

            this.validator = roomTemplate.validatorFactory(this);
            this.entranceBuilder = roomTemplate.entranceBuilderFactory();

            this.color = roomTemplate.color;

            cells = new RoomCells();
            blocks = new RoomBlocks();
        }

        public Room(RoomTemplate roomTemplate, List<RoomCell> roomCellList) : this(roomTemplate)
        {
            cells.Add(roomCellList);
            Reset();
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

        public void OnDestroy()
        {
        }

        public void Reset()
        {
            ResetRoomCellOrientations();
            ResetRoomEntrances();
        }

        /* 
            RoomBlock
        */
        public void AddBlock(RoomCells roomBlock)
        {
            blocks.Add(roomBlock);
            cells.Add(roomBlock);
        }

        public void AddBlocks(RoomBlocks roomBlocks)
        {
            blocks.Add(roomBlocks);

            foreach (RoomCells roomBlock in roomBlocks.blocks)
            {
                cells.Add(roomBlock);
            }
        }

        public void RemoveBlock(RoomCells block)
        {
            cells.cells.RemoveAll(cell => block.cells.Contains(cell));
            this.blocks.Remove(block);
        }

        public bool ContainsBlock(RoomCells roomBlock)
        {
            foreach (RoomCells block in blocks.blocks)
            {
                if (block == roomBlock)
                {
                    return true;
                }
            }

            return false;
        }

        public RoomCells FindBlockByCellCoordinates(CellCoordinates cellCoordinates)
        {
            foreach (RoomCells block in blocks.blocks)
            {
                foreach (RoomCell cell in block.cells)
                {
                    if (cell.coordinates.Matches(cellCoordinates))
                    {
                        return block;
                    }
                }
            }

            return null;
        }

        /* 
            RoomCells
        */

        // TODO - this feels like a weird place for this to live, perhaps it should be in State instead
        public void CalculateRoomCells(CellCoordinates blockCount)
        {
            RoomCells newCells = new RoomCells();
            RoomBlocks newBlocks = new RoomBlocks();

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

                    newBlocks.Add(blockCells);
                    newCells.Add(blockCells);
                }
            }

            this.cells = newCells;
            this.blocks = newBlocks;

            Reset();
        }

        public void ResetRoomCellOrientations()
        {
            foreach (RoomCell roomCell in cells.cells)
            {
                SetRoomCellOrientation(roomCell);
            }

            void SetRoomCellOrientation(RoomCell roomCell)
            {
                CellCoordinates coordinates = roomCell.coordinates;

                List<RoomCellOrientation> result = new List<RoomCellOrientation>();

                if (!cells.Contains(new CellCoordinates(coordinates.x, coordinates.floor + 1)))
                {
                    result.Add(RoomCellOrientation.Top);
                }

                if (!cells.Contains(new CellCoordinates(coordinates.x + 1, coordinates.floor)))
                {
                    result.Add(RoomCellOrientation.Right);
                }

                if (!cells.Contains(new CellCoordinates(coordinates.x, coordinates.floor - 1)))
                {
                    result.Add(RoomCellOrientation.Bottom);
                }

                if (!cells.Contains(new CellCoordinates(coordinates.x - 1, coordinates.floor)))
                {
                    result.Add(RoomCellOrientation.Left);
                }

                roomCell.orientation = result;
            }
        }

        CellCoordinates GetRelativeCoordinates(RoomCell roomCell)
        {
            return roomCell.coordinates.Subtract(new CellCoordinates(
                cells.GetLowestX(),
                cells.GetLowestFloor()
            ));
        }

        /* 
            RoomEntrances
        */
        public void RemoveEntrance(RoomEntrance entrance)
        {
            entrances.Remove(entrance);
        }

        public void ResetRoomEntrances()
        {
            DestroyRoomEntrances();
            CreateRoomEntrances();
        }

        void CreateRoomEntrances()
        {
            entrances = entranceBuilder.BuildRoomEntrances(cells);
        }

        void DestroyRoomEntrances()
        {
            entrances = null;
        }


        /*
            Furniture 
         */
        void InitializeFurniture() { }
    }
}



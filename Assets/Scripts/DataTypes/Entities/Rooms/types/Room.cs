using System.Collections.Generic;
using Newtonsoft.Json;
using TowerBuilder.DataTypes.Entities.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Entities.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms
{
    public partial class Room : Entity<Room.Key>
    {
        // TODO - move this up to entity
        public enum Resizability
        {
            Inflexible,
            Horizontal,
            Vertical,
            Flexible,
        }

        public enum Key
        {
            Default,
            Wheels
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Default, "Default" },
                { Key.Wheels,  "Wheels" },
            }
        );


        public override string idKey => "Rooms";

        public Resizability resizability { get; } = Resizability.Inflexible;

        public Dimensions blockDimensions { get; } = Dimensions.one;

        public int cellsInBlock
        {
            get => blockDimensions.width * blockDimensions.height;
        }

        // TODO - get rid of this
        public CellCoordinates bottomLeftCoordinates;

        public RoomBlocks blocks;

        public RoomValidatorBase validator { get; }
        public RoomFurnitureBuilderBase furnitureBuilder { get; }

        public Skin skin;

        public override int pricePerCell { get => skin.config.pricePerCell; }

        public Room(RoomTemplate roomTemplate) : base(roomTemplate)
        {
            this.resizability = roomTemplate.resizability;
            this.blockDimensions = roomTemplate.blockDimensions;

            this.validator = roomTemplate.validatorFactory(this);
            this.furnitureBuilder = roomTemplate.furnitureBuilderFactory(this);

            this.skin = new Skin(roomTemplate.skinKey);

            blocks = new RoomBlocks();
        }

        public override string ToString() => $"room {id}";

        public void Reset()
        {
            // blocks.cells.Refresh();
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
            RoomBlocks newBlocks = new RoomBlocks();

            for (int x = 0; x < blockCount.x; x++)
            {
                for (int floor = 0; floor < blockCount.floor; floor++)
                {
                    RoomCells blockCells = new RoomCells(
                        CellCoordinatesList.CreateRectangle(
                            blockDimensions.width,
                            blockDimensions.height
                        )
                    );

                    blockCells.PositionAtCoordinates(new CellCoordinates(
                        bottomLeftCoordinates.x + (blockDimensions.width * x),
                        bottomLeftCoordinates.floor + (blockDimensions.height * floor)
                    ));

                    newBlocks.Add(blockCells);
                }
            }

            this.blocks = newBlocks;

            Reset();
        }
    }
}



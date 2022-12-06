using Newtonsoft.Json;
using TowerBuilder.DataTypes.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class Room
    {
        public const int PRICE_PER_CELL = 500;

        public enum SkinKey
        {
            Default,
            Wheels
        }

        public enum Resizability
        {
            Inflexible,
            Horizontal,
            Vertical,
            Flexible,
        }

        public int id { get; private set; }

        public string title { get; private set; } = "None";
        public string key { get; private set; } = "None";
        public string category { get; private set; } = "None";

        public Resizability resizability = Resizability.Inflexible;

        public Dimensions blockDimensions { get; private set; } = Dimensions.one;

        public int cellsInBlock { get => blockDimensions.width * blockDimensions.height; }

        // Saved rooms should never be in blueprint mode
        [JsonIgnore]
        public bool isInBlueprintMode = false;

        public CellCoordinates bottomLeftCoordinates;

        public RoomBlocks blocks;

        public RoomValidatorBase validator { get; private set; }
        public RoomFurnitureBuilderBase furnitureBuilder { get; private set; }

        public SkinKey skinKey;

        public RoomTemplate roomTemplate;

        [JsonIgnore]
        public int price { get => PRICE_PER_CELL * cellsInBlock * this.blocks.blocks.Count; }

        public Room(RoomTemplate roomTemplate)
        {
            this.id = UIDGenerator.Generate("Room");

            this.title = roomTemplate.title;
            this.key = roomTemplate.key;
            this.category = roomTemplate.category;
            this.resizability = roomTemplate.resizability;
            this.blockDimensions = roomTemplate.blockDimensions;

            this.roomTemplate = roomTemplate;

            this.validator = roomTemplate.validatorFactory(this);
            this.furnitureBuilder = roomTemplate.furnitureBuilderFactory(this);

            this.skinKey = roomTemplate.skinKey;

            blocks = new RoomBlocks();
        }

        public override string ToString()
        {
            return $"room {id}";
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public void OnDestroy() { }

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



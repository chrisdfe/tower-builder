using System.Collections.Generic;
using Newtonsoft.Json;
using TowerBuilder.DataTypes.Rooms.FurnitureBuilders;
using TowerBuilder.DataTypes.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms
{
    public class Room
    {
        public const int PRICE_PER_CELL = 500;


        public enum Resizability
        {
            Inflexible,
            Horizontal,
            Vertical,
            Flexible,
        }

        public class Skin
        {
            public enum Key
            {
                Default,
                Wheels
            }

            public class Config
            {
                public int pricePerCell;
                public bool hasInteriorLights;
            }

            public Key key { get; }
            public Config config { get; }

            public Skin(Key key)
            {
                this.key = key;
                this.config = ConfigMap.Get(key);
            }

            public static class ConfigMap
            {
                static Dictionary<Key, Config> map = new Dictionary<Key, Config>() {
                {
                    Key.Default,
                    new Config() {
                        pricePerCell = 500,
                        hasInteriorLights = true
                    }
                },
                {
                    Key.Wheels,
                    new Config() {
                        pricePerCell = 1500,
                        hasInteriorLights = false
                    }
                },
            };

                public static Config Get(Key key) => map.GetValueOrDefault(key);
            }
        }

        public int id { get; }

        public string title { get; } = "None";
        public string key { get; } = "None";
        public string category { get; } = "None";

        public Resizability resizability { get; } = Resizability.Inflexible;

        public Dimensions blockDimensions { get; } = Dimensions.one;

        public int cellsInBlock
        {
            get => blockDimensions.width * blockDimensions.height;
        }

        // Saved rooms should never be in blueprint mode
        [JsonIgnore]
        public bool isInBlueprintMode = false;

        // TODO - get rid of this
        public CellCoordinates bottomLeftCoordinates;

        public RoomBlocks blocks;

        public RoomValidatorBase validator { get; }
        public RoomFurnitureBuilderBase furnitureBuilder { get; }

        public Skin skin;

        public RoomTemplate roomTemplate;

        public int pricePerCell { get => skin.config.pricePerCell; }

        [JsonIgnore]
        public int price { get => pricePerCell * cellsInBlock * this.blocks.blocks.Count; }

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

            this.skin = new Skin(roomTemplate.skinKey);

            blocks = new RoomBlocks();
        }

        public override string ToString() => $"room {id}";

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



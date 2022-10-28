
using System.Collections.Generic;
using TowerBuilder.DataTypes.Rooms.Validators;
using TowerBuilder.State;
using TowerBuilder.Utils;


namespace TowerBuilder.DataTypes.Rooms.Blueprints
{
    public class Blueprint
    {
        public List<BlueprintCell> roomBlueprintCells;
        public List<RoomValidationError> validationErrors { get; private set; }

        public Room room { get; private set; }
        public RoomTemplate roomTemplate;

        public CellCoordinates buildStartCoordinates { get; private set; } = new CellCoordinates(0, 0);
        public CellCoordinates buildEndCoordinates { get; private set; } = new CellCoordinates(0, 0);

        // TODO - use this property as a cache and/or have a GetSelectionBox method
        SelectionBox selectionBox
        {
            get
            {
                return new SelectionBox(buildStartCoordinates, buildEndCoordinates);
            }
        }

        public Blueprint(RoomTemplate roomTemplate, CellCoordinates buildStartCoordinates)
        {
            this.roomTemplate = roomTemplate;
            this.room = new Room(roomTemplate);
            this.room.isInBlueprintMode = true;

            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();

            validationErrors = new List<RoomValidationError>();

            ResetBlueprintCells();
        }

        public void OnDestroy() { }

        public void Reset()
        {
            this.room = new Room(this.roomTemplate);
            this.room.isInBlueprintMode = true;

            SetRoomCells();
            ResetBlueprintCells();
        }

        public void SetRoomTemplate(RoomTemplate roomTemplate)
        {
            this.roomTemplate = roomTemplate;
            this.room = new Room(roomTemplate);
            this.room.isInBlueprintMode = true;
            SetRoomCells();
            ResetBlueprintCells();
        }

        public void SetBuildStartCell(CellCoordinates buildStartCoordinates)
        {
            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();

            ResetBlueprintCells();
        }

        public void SetBuildEndCell(CellCoordinates buildEndCoordinates)
        {
            this.buildEndCoordinates = buildEndCoordinates;
            ResetBlueprintCells();
        }

        public void SetRoomCells()
        {
            CellCoordinates blockCount = new CellCoordinates(1, 1);

            if (room.resizability.Matches(RoomResizability.Inflexible()))
            {
                room.bottomLeftCoordinates = buildStartCoordinates;
            }
            else
            {
                // Restrict resizability to X/Y depending on RoomFlexibility
                if (room.resizability.x)
                {
                    room.bottomLeftCoordinates = new CellCoordinates(
                        selectionBox.bottomLeft.x,
                        buildStartCoordinates.floor
                    );
                    blockCount.x = MathUtils.RoundUpToNearest(selectionBox.dimensions.width, room.blockDimensions.width);
                }

                if (room.resizability.floor)
                {
                    room.bottomLeftCoordinates = new CellCoordinates(
                        buildStartCoordinates.x,
                        selectionBox.bottomLeft.floor
                    );
                    blockCount.floor = MathUtils.RoundUpToNearest(selectionBox.dimensions.height, room.blockDimensions.height);
                }
            }

            this.room.CalculateRoomCells(blockCount);
        }

        public List<RoomValidationError> Validate(AppState stores)
        {
            validationErrors = new List<RoomValidationError>();

            validationErrors = room.validator.Validate(room, stores);

            return validationErrors;
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }

        void ResetBlueprintCells()
        {
            SetRoomCells();

            // TODO - clean up existing roomBlueprintCells first?
            roomBlueprintCells = new List<BlueprintCell>();

            foreach (RoomCell roomCell in room.cells.cells)
            {
                BlueprintCell newBlueprintCell = new BlueprintCell(this, roomCell);
                roomBlueprintCells.Add(newBlueprintCell);
            }
        }
    }
}



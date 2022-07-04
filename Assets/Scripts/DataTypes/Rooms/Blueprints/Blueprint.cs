
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

        public CellCoordinates buildStartCoordinates { get; private set; } = new CellCoordinates(0, 0);
        public CellCoordinates buildEndCoordinates { get; private set; } = new CellCoordinates(0, 0);

        SelectionBox selectionBox
        {
            get
            {
                return new SelectionBox(buildStartCoordinates, buildEndCoordinates);
            }
        }

        public Blueprint(RoomTemplate roomTemplate, CellCoordinates buildStartCoordinates)
        {
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
            this.room = new Room(this.room.roomTemplate);
            SetRoomCells();
            ResetBlueprintCells();
            this.room.isInBlueprintMode = true;
        }

        public void SetRoomTemplate(RoomTemplate roomTemplate)
        {
            room.SetTemplate(roomTemplate);
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
            RoomTemplate roomTemplate = room.roomTemplate;

            if (roomTemplate == null)
            {
                return;
            }

            CellCoordinates blockCount = new CellCoordinates(1, 1);

            if (roomTemplate.resizability.Matches(RoomResizability.Inflexible()))
            {
                room.bottomLeftCoordinates = buildStartCoordinates;
            }
            else
            {
                // Restrict resizability to X/Y depending on RoomFlexibility
                if (roomTemplate.resizability.x)
                {
                    room.bottomLeftCoordinates = new CellCoordinates(
                        selectionBox.bottomLeft.x,
                        buildStartCoordinates.floor
                    );
                    blockCount.x = MathUtils.RoundUpToNearest(selectionBox.dimensions.width, roomTemplate.blockDimensions.width);
                }

                if (roomTemplate.resizability.floor)
                {
                    room.bottomLeftCoordinates = new CellCoordinates(
                        buildStartCoordinates.x,
                        selectionBox.bottomLeft.floor
                    );
                    blockCount.floor = MathUtils.RoundUpToNearest(selectionBox.dimensions.height, roomTemplate.blockDimensions.height);
                }
            }

            room.blockCount = blockCount;

            this.room.CalculateRoomCells();
        }

        public List<RoomValidationError> Validate(AppState stores)
        {
            validationErrors = new List<RoomValidationError>();

            if (room.roomTemplate != null)
            {
                RoomValidatorBase validator = room.roomTemplate.validatorFactory();
                validationErrors = validator.Validate(room, stores);
            }

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

            foreach (RoomCell roomCell in room.roomCells.cells)
            {
                BlueprintCell newBlueprintCell = new BlueprintCell(this, roomCell);
                roomBlueprintCells.Add(newBlueprintCell);
            }
        }
    }
}



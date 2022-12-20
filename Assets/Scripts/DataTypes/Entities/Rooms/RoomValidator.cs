using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Wheels;

namespace TowerBuilder.DataTypes.Entities.Rooms.Validators
{
    public class RoomValidator : EntityValidator
    {
        protected override List<EntityValidationFunc> customValidators =>
            new List<EntityValidationFunc>()
            {
                GenericEntityValidations.CreateValidateEntityCellIsNotOnFloor(0),
                ValidateRoomAboveOtherRoomOrWheels,
                // ValidateAcceptableOverhang,
            };

        protected override List<EntityValidationFunc> baseValidatorIgnoreList => new List<EntityValidationFunc>()
        {
            // To allow rooms to be extended by building another room on top of it
            GenericEntityValidations.ValidateEntityIsNotOverlappingAnotherEntity,
        };

        public RoomValidator(Room room) : base(room) { }

        public static EntityValidationErrorList ValidateRoomAboveOtherRoomOrWheels(AppState appState, Entity entity)
        {
            CellCoordinatesList bottomRow = entity.cellCoordinatesList.bottomRow;

            foreach (CellCoordinates cellCoordinates in bottomRow.items)
            {
                Room roomBelow = appState.Entities.Rooms.queries
                    .FindEntityTypeAtCell(cellCoordinates.coordinatesBelow);

                Wheel wheelsBelow = appState.Entities.Wheels.queries
                    .FindEntityTypeAtCell(cellCoordinates.coordinatesBelow);

                if (roomBelow == null && wheelsBelow == null)
                {
                    return new EntityValidationErrorList("Room must be built above other room or wheels");
                }
            }

            return new EntityValidationErrorList();
        }

        /*
        public static EntityValidationErrorList ValidateAcceptableOverhang(AppState appState, Entity entity, CellCoordinates cellCoordinates)
        {
            Room room = entity as Room;
            bool isOnBottom = room.cellCoordinatesList.lowestFloor == 0;

            // TODO - account for room width being less than MAX_OVERHANG
            int roomWidth = room.cellCoordinatesList.width;

            if (isOnBottom && cellCoordinates.floor > 0)
            {
                Room roomUnderneath = appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelow);

                if (roomUnderneath == null)
                {
                    // cell is overhanging - look for rooms underneath within acceptable overhang range
                    Room roomUnderneathToTheLeft = appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelowLeft);
                    Room roomUnderneathToTheRight = appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelowRight);

                    if (roomUnderneathToTheLeft == null && roomUnderneathToTheRight == null)
                    {
                        return new EntityValidationErrorList($"Rooms must have a maximum overhang of 1 cell.");
                    }
                }

            }

            return new EntityValidationErrorList();
        }
        */
    }
}
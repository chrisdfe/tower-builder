using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Wheels;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.EntityGroups.Rooms.Validators
{
    public class RoomValidator : Validator<Room>
    {
        protected override List<Validator<Room>.ValidationFunc> customValidators =>
            new List<Validator<Room>.ValidationFunc>()
            {
                // GenericEntityValidations.CreateValidateEntityCellIsNotOnFloor(0),
                // ValidateRoomAboveOtherRoomOrWheels,
                // ValidateAcceptableOverhang,
            };

        // protected override List<EntityValidator.ValidationFunc> baseValidatorIgnoreList => new List<EntityValidator.ValidationFunc>()
        // {
        // To allow rooms to be extended by building another room on top of it
        // GenericEntityValidations.ValidateEntityIsNotOverlappingAnotherEntity,
        // };

        public RoomValidator(Room room) : base(room) { }

        public static ListWrapper<ValidationError> ValidateRoomAboveOtherRoomOrWheels(AppState appState, Room room)
        {
            // CellCoordinatesList bottomRow = entity.cellCoordinatesList.bottomRow;

            // foreach (CellCoordinates cellCoordinates in bottomRow.items)
            // {
            //     Room roomBelow = appState.Entities.Rooms.queries
            //         .FindEntityTypeAtCell(cellCoordinates.coordinatesBelow);

            //     Wheel wheelsBelow = appState.Entities.Wheels.queries
            //         .FindEntityTypeAtCell(cellCoordinates.coordinatesBelow);

            //     if (roomBelow == null && wheelsBelow == null)
            //     {
            //         return Validator.CreateSingleItemValidationErrorList("Room must be built above other room or wheels");
            //     }
            // }

            return new ListWrapper<ValidationError>();
        }

        /*
        public static ListWrapper<ValidationError> ValidateAcceptableOverhang(AppState appState, Entity entity, CellCoordinates cellCoordinates)
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
                        return new ListWrapper<ValidationError>($"Rooms must have a maximum overhang of 1 cell.");
                    }
                }

            }

            return new ListWrapper<ValidationError>();
        }
        */
    }
}
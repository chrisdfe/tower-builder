using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public class WheelsRoomValidator : RoomValidatorBase
    {
        public WheelsRoomValidator(Room room) : base(room) { }

        protected override List<RoomCellValidationFunc> roomCellValidators
        {
            get
            {
                return new List<RoomCellValidationFunc>() {
                    GenericRoomCellValidations.CreateValidateRoomCellIsOnFloor(0)
            };
            }
        }

        public override bool isAllowedOnGroundFloor { get { return true; } }

        List<RoomValidationError> ValidateWheelsAreOnCorrectFloor(AppState appState, Room room, RoomCell roomCell)
        {
            List<RoomValidationError> errors = new List<RoomValidationError>();

            CellCoordinates cellCoordinates = roomCell.coordinates;

            // Wheels must be on floor 0
            // Since wheels can be 1-2 tiles high, make sure the cell we're validating here is the bottom-most cell
            bool isOnBottom = room.blocks.cells.GetRelativeRoomCellCoordinates(roomCell).floor == 0;
            if (isOnBottom && cellCoordinates.floor != 0)
            {
                errors.Add(new RoomValidationError("Wheels must be placed on first floor"));
            }

            return errors;
        }
    }
}
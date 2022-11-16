using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public class LobbyRoomValidator : RoomValidatorBase
    {
        public LobbyRoomValidator(Room room) : base(room) { }

        protected override List<GenericRoomCellValidations.ValidationFunc> RoomCellValidators
        {
            get
            {
                return new List<GenericRoomCellValidations.ValidationFunc>() {
                    ValidateLobbyIsOnCorrectFloor
                };
            }
        }

        List<RoomValidationError> ValidateLobbyIsOnCorrectFloor(AppState appState, Room room, RoomCell roomCell)
        {
            List<RoomValidationError> errors = new List<RoomValidationError>();

            CellCoordinates cellCoordinates = roomCell.coordinates;

            // Lobbies must be on floor 0
            // Since lobbies can be 1-2 tiles high, make sure the cell we're validating here is the bottom-most cell
            bool isOnBottom = room.blocks.cells.GetRelativeRoomCellCoordinates(roomCell).floor == 0;
            if (isOnBottom && cellCoordinates.floor != 0)
            {
                errors.Add(new RoomValidationError("Lobbies must be placed on first floor"));
            }

            return errors;
        }
    }
}
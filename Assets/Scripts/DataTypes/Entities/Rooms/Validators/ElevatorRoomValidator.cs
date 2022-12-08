using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms.Validators
{
    public class ElevatorRoomValidator : RoomValidatorBase
    {
        public ElevatorRoomValidator(Room room) : base(room) { }

        protected override List<RoomCellValidationFunc> roomCellValidators
        {
            get
            {
                return new List<RoomCellValidationFunc>() {
                    ValidateElevatorDistance
                };
            }
        }

        List<RoomValidationError> ValidateElevatorDistance(AppState appState, Room room, CellCoordinates cellCoordinates)
        {
            List<RoomValidationError> errors = new List<RoomValidationError>();

            RoomList allRooms = appState.Rooms.list;

            // Elevators can't be too close together
            // TODO - check above + to the left + right and below + to the left and right (not directly above or below, that's ok)
            Room leftRoom = allRooms.FindRoomAtCell(cellCoordinates.coordinatesLeft);
            Room rightRoom = allRooms.FindRoomAtCell(cellCoordinates.coordinatesRight);

            if (
                (leftRoom != null && leftRoom.template.category == "Elevator") ||
                (rightRoom != null && rightRoom.template.category == "Elevator")
            )
            {
                errors.Add(
                    new RoomValidationError("Elevators cannot be placed direclty next to each other.")
                );
            }

            return errors;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
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

        List<RoomValidationError> ValidateElevatorDistance(AppState appState, Room room, RoomCell roomCell)
        {
            List<RoomValidationError> errors = new List<RoomValidationError>();

            CellCoordinates cellCoordinates = roomCell.coordinates;

            RoomList allRooms = appState.Rooms.roomList;

            // Elevators can't be too close together
            // TODO - check above + to the left + right and below + to the left and right (not directly above or below, that's ok)
            Room leftRoom = allRooms.FindRoomAtCell(new CellCoordinates(cellCoordinates.x - 1, cellCoordinates.floor));
            Room rightRoom = allRooms.FindRoomAtCell(new CellCoordinates(cellCoordinates.x + 1, cellCoordinates.floor));

            if (
                (leftRoom != null && leftRoom.category == "Elevator") ||
                (rightRoom != null && rightRoom.category == "Elevator")
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
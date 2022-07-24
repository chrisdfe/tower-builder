using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public class ElevatorRoomValidator : RoomValidatorBase
    {
        public override List<RoomValidationError> ValidateRoomCell(RoomCell roomCell, AppState stores)
        {
            List<RoomValidationError> result = base.ValidateRoomCell(roomCell, stores);

            CellCoordinates cellCoordinates = roomCell.coordinates;

            RoomList allRooms = stores.Rooms.roomList;

            // Elevators can't be too close together
            // TODO - check above + to the left + right and below + to the left and right (not directly above or below, that's ok)
            Room leftRoom = allRooms.FindRoomAtCell(new CellCoordinates(cellCoordinates.x - 1, cellCoordinates.floor));
            Room rightRoom = allRooms.FindRoomAtCell(new CellCoordinates(cellCoordinates.x + 1, cellCoordinates.floor));

            if (
                (leftRoom != null && leftRoom.category == "Elevator") ||
                (rightRoom != null && rightRoom.category == "Elevator")
            )
            {
                result.Add(
                    new RoomValidationError("Elevators cannot be placed direclty next to each other.")
                );
            }

            return result;
        }
    }
}
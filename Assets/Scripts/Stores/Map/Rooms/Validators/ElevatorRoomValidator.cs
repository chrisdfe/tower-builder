using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Validators
{
    public class ElevatorRoomValidator : RoomValidatorBase
    {
        public override List<RoomValidationError> ValidateRoomCell(RoomCell roomCell, StoreRegistry stores)
        {
            List<RoomValidationError> result = base.ValidateRoomCell(roomCell, stores);

            CellCoordinates cellCoordinates = roomCell.coordinates;

            RoomList allRooms = stores.Map.rooms;

            // Elevators can't be too close together
            // TODO - check above + to the left + right and below + to the left and right (not directly above or below, that's ok)
            Room leftRoom = allRooms.FindRoomAtCell(new CellCoordinates(cellCoordinates.x - 1, cellCoordinates.floor));
            Room rightRoom = allRooms.FindRoomAtCell(new CellCoordinates(cellCoordinates.x + 1, cellCoordinates.floor));

            if (
                (leftRoom != null && leftRoom.roomDetails.category == RoomCategory.Elevator) ||
                (rightRoom != null && rightRoom.roomDetails.category == RoomCategory.Elevator)
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
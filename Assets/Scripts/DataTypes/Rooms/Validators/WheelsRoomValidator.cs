using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public class WheelsRoomValidator : RoomValidatorBase
    {
        public override List<RoomValidationError> ValidateRoomCell(RoomCell roomCell, AppState stores)
        {
            List<RoomValidationError> result = base.ValidateRoomCell(roomCell, stores);

            RoomList allRooms = stores.Rooms.roomList;

            CellCoordinates cellCoordinates = roomCell.coordinates;

            // Wheels must be on floor 0
            // Since wheels can be 1-2 tiles high, make sure the cell we're validating here is the bottom-most cell
            bool isOnBottom = roomCell.GetRelativeCoordinates().floor == 0;
            if (isOnBottom && cellCoordinates.floor != 0)
            {
                result.Add(new RoomValidationError("Wheels must be placed on first floor"));
            }

            return result;
        }
    }
}
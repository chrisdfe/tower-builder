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
    public class LobbyRoomValidator : RoomValidatorBase
    {
        public override List<RoomValidationError> ValidateRoomCell(RoomCell roomCell, StoreRegistry stores)
        {
            List<RoomValidationError> result = base.ValidateRoomCell(roomCell, stores);

            RoomList allRooms = stores.Map.rooms;

            CellCoordinates cellCoordinates = roomCell.coordinates;

            // Lobbies must be on floor 0
            // Since lobbies can be 1-2 tiles high, make sure the cell we're validating here is the bottom-most cell
            bool isOnBottom = roomCell.GetRelativeCoordinates().floor == 0;
            if (isOnBottom && cellCoordinates.floor != 0)
            {
                result.Add(new RoomValidationError("Lobbies must be placed on first floor"));
            }

            return result;
        }
    }
}
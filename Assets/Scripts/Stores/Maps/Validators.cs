using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public static class Validators
    {
        public static List<RoomBlueprintValidationError> ValidateBlueprintCell(RoomBlueprintCell roomBlueprintCell, List<Room> allRooms)
        {
            List<RoomBlueprintValidationError> validationErrors = new List<RoomBlueprintValidationError>();

            // Check for overlapping cells
            foreach (Room room in allRooms)
            {
                foreach (CellCoordinates otherRoomCellCoordinates in room.roomCells)
                {
                    if (otherRoomCellCoordinates.Matches(roomBlueprintCell.cellCoordinates))
                    {
                        validationErrors.Add(new RoomBlueprintValidationError("You cannot build rooms on top of each other."));
                    }
                }
            }

            List<RoomBlueprintValidationError> roomSpecificValidationErrors = ValidateForRoomType(roomBlueprintCell, allRooms);

            return validationErrors.Concat(roomSpecificValidationErrors).ToList();
        }

        // Room-specific cell validations
        // TODO - this should ultimately live with the rest of Room stuff when I rewrite Rooms in a more
        //        object-oriented way
        //        for now all room-specific validations will just go here I guess
        public static List<RoomBlueprintValidationError> ValidateForRoomType(RoomBlueprintCell roomBlueprintCell, List<Room> allRooms)
        {
            List<RoomBlueprintValidationError> result = new List<RoomBlueprintValidationError>();

            RoomKey roomKey = roomBlueprintCell.parentBlueprint.roomKey;
            MapRoomDetails roomDetails = Room.GetDetails(roomKey);
            CellCoordinates cellCoordinates = roomBlueprintCell.cellCoordinates;

            if (roomKey == RoomKey.Lobby)
            {
                // Lobbies must be on floor 0
                if (cellCoordinates.floor != 0)
                {
                    result.Add(new RoomBlueprintValidationError("Lobbies must be placed on first floor"));
                }
            }
            else if (roomKey == RoomKey.Elevator)
            {
                // elevators can't be too close together
                Room leftRoom = MapStore.Helpers.FindRoomAtCell(new CellCoordinates(cellCoordinates.x - 1, cellCoordinates.floor), allRooms);
                Room rightRoom = MapStore.Helpers.FindRoomAtCell(new CellCoordinates(cellCoordinates.x + 1, cellCoordinates.floor), allRooms);

                if (
                    (leftRoom != null && leftRoom.roomKey == RoomKey.Elevator) ||
                    (rightRoom != null && rightRoom.roomKey == RoomKey.Elevator)
                )
                {
                    result.Add(new RoomBlueprintValidationError("Elevators cannot be placed direclty next to each other."));
                }
            }

            return result;
        }
    }
}
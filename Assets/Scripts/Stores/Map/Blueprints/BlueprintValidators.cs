using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Blueprints
{
    public static class BlueprintValidators
    {
        public static List<BlueprintValidationError> ValidateBlueprintCell(BlueprintCell roomBlueprintCell, RoomList allRooms)
        {
            List<BlueprintValidationError> validationErrors = new List<BlueprintValidationError>();

            // Check for overlapping cells
            foreach (Room room in allRooms.rooms)
            {
                foreach (RoomCell otherRoomCell in room.roomCells.cells)
                {
                    if (otherRoomCell.coordinates.Matches(roomBlueprintCell.roomCell.coordinates))
                    {
                        validationErrors.Add(
                            new BlueprintValidationError("You cannot build rooms on top of each other.")
                        );
                    }
                }
            }

            List<BlueprintValidationError> roomSpecificValidationErrors = ValidateForRoomType(roomBlueprintCell, allRooms);

            return validationErrors.Concat(roomSpecificValidationErrors).ToList();
        }

        // Room-specific cell validations
        // TODO - this should ultimately live with the rest of Room stuff when I rewrite Rooms in a more
        //        object-oriented way but for now all room-specific validations will just go here I guess
        public static List<BlueprintValidationError> ValidateForRoomType(BlueprintCell roomBlueprintCell, RoomList allRooms)
        {
            List<BlueprintValidationError> result = new List<BlueprintValidationError>();

            RoomDetails roomDetails = roomBlueprintCell.parentBlueprint.room.roomDetails;
            RoomCategory roomCategory = roomDetails.category;
            CellCoordinates cellCoordinates = roomBlueprintCell.roomCell.coordinates;

            if (roomCategory == RoomCategory.Lobby)
            {
                // Lobbies must be on floor 0
                // Since lobbies can be 1-2 tiles high, make sure the cell we're validating here is the bottom-most cell
                bool isOnBottom = roomBlueprintCell.relativeCellCoordinates.floor == 0;
                if (isOnBottom && cellCoordinates.floor != 0)
                {
                    result.Add(new BlueprintValidationError("Lobbies must be placed on first floor"));
                }
            }
            else if (roomCategory == RoomCategory.Elevator)
            {
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
                        new BlueprintValidationError("Elevators cannot be placed direclty next to each other.")
                    );
                }
            }

            return result;
        }
    }
}
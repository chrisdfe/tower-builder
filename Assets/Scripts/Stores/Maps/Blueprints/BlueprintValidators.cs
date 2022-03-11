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
                    if (otherRoomCell.coordinates.Matches(roomBlueprintCell.cellCoordinates))
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

            RoomKey roomKey = roomBlueprintCell.parentBlueprint.roomKey;
            RoomDetails roomDetails = Room.GetDetails(roomKey);
            CellCoordinates cellCoordinates = roomBlueprintCell.cellCoordinates;

            if (roomKey == RoomKey.Lobby)
            {
                // Lobbies must be on floor 0
                if (cellCoordinates.floor != 0)
                {
                    result.Add(new BlueprintValidationError("Lobbies must be placed on first floor"));
                }
            }
            else if (roomKey == RoomKey.Elevator)
            {
                // elevators can't be too close together
                Room leftRoom = allRooms.FindRoomAtCell(new CellCoordinates(cellCoordinates.x - 1, cellCoordinates.floor));
                Room rightRoom = allRooms.FindRoomAtCell(new CellCoordinates(cellCoordinates.x + 1, cellCoordinates.floor));

                if (
                    (leftRoom != null && leftRoom.roomKey == RoomKey.Elevator) ||
                    (rightRoom != null && rightRoom.roomKey == RoomKey.Elevator)
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
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public static class GenericRoomValidations
    {
        public delegate List<RoomValidationError> ValidationFunc(AppState appState, Room room);

        public static List<RoomValidationError> ValidateWallet(AppState appState, Room room)
        {
            List<RoomValidationError> errors = new List<RoomValidationError>();
            int walletBalance = appState.Wallet.balance;

            if (walletBalance < room.price)
            {
                errors.Add(new RoomValidationError("Insufficient Funds."));
            }

            return errors;
        }
    }

    public static class GenericRoomCellValidations
    {
        public delegate List<RoomValidationError> ValidationFunc(AppState appState, Room room, RoomCell roomCell);

        public static List<RoomValidationError> ValidateOverlap(AppState appState, Room room, RoomCell roomCell)
        {
            List<RoomValidationError> errors = new List<RoomValidationError>();

            RoomList allRooms = appState.Rooms.roomList;

            // Check for overlapping cells
            foreach (Room otherRoom in allRooms.rooms)
            {
                if (otherRoom != room && otherRoom.blocks.cells.coordinatesList.Contains(roomCell.coordinates))
                {
                    errors.Add(
                        new RoomValidationError("You cannot build rooms on top of each other.")
                    );
                }
            }

            return errors;
        }
    }
}
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public static class GenericRoomValidations
    {
        public static List<RoomValidationError> ValidateWallet(AppState appState, Room room)
        {
            return GenericValidations.ValidateWalletHasEnoughMoney<RoomValidationError>(appState, room.price);
        }
    }

    public static class GenericRoomCellValidations
    {
        public static List<RoomValidationError> ValidateRoomCellIsNotOverlappingAnotherRoom(AppState appState, Room room, RoomCell roomCell)
        {
            List<RoomValidationError> errors = new List<RoomValidationError>();

            RoomList allRooms = appState.Rooms.roomList;

            // Check for overlapping cells
            foreach (Room otherRoom in allRooms.items)
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

        public static RoomCellValidationFunc CreateValidateRoomCellIsOnFloor(int floor)
        {
            return (AppState appState, Room room, RoomCell roomCell) =>
            {
                // Since rooms can be multiple tiles high, make sure the cell we're validating here is the bottom-most cell
                bool isOnBottom = room.blocks.cells.GetRelativeRoomCellCoordinates(roomCell).floor == 0;
                if (isOnBottom && roomCell.coordinates.floor != 0)
                {
                    return Helpers.CreateErrorList($"{room.title} must be placed on floor {floor + 1}");
                }

                return Helpers.CreateEmptyErrorList();
            };
        }

        public static RoomCellValidationFunc CreateValidateRoomCellIsNotOnFloor(int floor)
        {
            return (AppState appState, Room room, RoomCell roomCell) =>
            {
                // Since rooms can be multiple tiles high, make sure the cell we're validating here is the bottom-most cell
                bool isOnBottom = room.blocks.cells.GetRelativeRoomCellCoordinates(roomCell).floor == 0;
                if (isOnBottom && roomCell.coordinates.floor == 0)
                {
                    return Helpers.CreateErrorList($"{room.title} must not be placed on floor {floor + 1}");
                }

                return Helpers.CreateEmptyErrorList();
            };
        }

        static class Helpers
        {
            public static List<RoomValidationError> CreateErrorList(string message)
            {
                return new List<RoomValidationError>() { new RoomValidationError(message) };
            }

            public static List<RoomValidationError> CreateEmptyErrorList()
            {
                return new List<RoomValidationError>();
            }
        }
    }
}
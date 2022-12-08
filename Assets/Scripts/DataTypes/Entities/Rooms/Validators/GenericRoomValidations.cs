using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms.Validators
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
        public static List<RoomValidationError> ValidateRoomCellIsNotOverlappingAnotherRoom(AppState appState, Room room, CellCoordinates cellCoordinates)
        {
            List<RoomValidationError> errors = new List<RoomValidationError>();

            RoomList allRooms = appState.Rooms.list;

            // Check for overlapping cells
            foreach (Room otherRoom in allRooms.items)
            {
                if (otherRoom != room && otherRoom.cellCoordinatesList.Contains(cellCoordinates))
                {
                    return Helpers.CreateErrorList("You cannot build rooms on top of each other.");
                }
            }

            return Helpers.CreateEmptyErrorList();
        }

        public static List<RoomValidationError> ValidateAcceptableOverhang(AppState appState, Room room, CellCoordinates cellCoordinates)
        {
            bool isOnBottom = room.cellCoordinatesList.lowestFloor == 0;

            // TODO - account for room width being less than MAX_OVERHANG
            int roomWidth = room.cellCoordinatesList.width;

            if (isOnBottom && cellCoordinates.floor > 0)
            {
                Room roomUnderneath = appState.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelow);

                if (roomUnderneath == null)
                {
                    // cell is overhanging - look for rooms underneath within acceptable overhang range
                    Room roomUnderneathToTheLeft = appState.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelowLeft);
                    Room roomUnderneathToTheRight = appState.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelowRight);

                    if (
                        roomUnderneathToTheLeft == null && roomUnderneathToTheRight == null
                    )
                    {
                        return Helpers.CreateErrorList($"Rooms must have a maximum overhang of 1 cell.");
                    }
                }

            }

            return Helpers.CreateEmptyErrorList();
        }

        public static List<RoomValidationError> ValidateRoomCellIsNotUnderground(AppState appState, Room room, CellCoordinates cellCoordinates)
        {
            if (cellCoordinates.floor < 0)
            {
                return Helpers.CreateErrorList("You cannot build rooms underground.");
            }

            return Helpers.CreateEmptyErrorList();
        }

        public static RoomCellValidationFunc CreateValidateRoomCellIsOnFloor(int floor) =>
            (AppState appState, Room room, CellCoordinates cellCoordinates) =>
            {
                // Since rooms can be multiple tiles high, make sure the cell we're validating here is the bottom-most cell
                bool isOnBottom = room.cellCoordinatesList.lowestFloor == 0;
                if (isOnBottom && cellCoordinates.floor != 0)
                {
                    return Helpers.CreateErrorList($"{room.template.title} must be placed on floor {floor + 1}");
                }

                return Helpers.CreateEmptyErrorList();
            };

        public static RoomCellValidationFunc CreateValidateRoomCellIsNotOnFloor(int floor) =>
            (AppState appState, Room room, CellCoordinates cellCoordinates) =>
            {
                // Since rooms can be multiple tiles high, make sure the cell we're validating here is the bottom-most cell
                bool isOnBottom = room.cellCoordinatesList.lowestFloor == 0;
                if (isOnBottom && cellCoordinates.floor == 0)
                {
                    return Helpers.CreateErrorList($"{room.template.title} must not be placed on floor {floor + 1}");
                }

                return Helpers.CreateEmptyErrorList();
            };

        static class Helpers
        {
            public static List<RoomValidationError> CreateErrorList(string message) =>
                new List<RoomValidationError>() { new RoomValidationError(message) };

            public static List<RoomValidationError> CreateEmptyErrorList() =>
                new List<RoomValidationError>();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
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

            RoomList allRooms = appState.Rooms.list;

            // Check for overlapping cells
            foreach (Room otherRoom in allRooms.items)
            {
                if (otherRoom != room && otherRoom.blocks.cells.coordinatesList.Contains(roomCell.coordinates))
                {
                    return Helpers.CreateErrorList("You cannot build rooms on top of each other.");
                }
            }

            return Helpers.CreateEmptyErrorList();
        }

        public static List<RoomValidationError> ValidateAcceptableOverhang(AppState appState, Room room, RoomCell roomCell)
        {
            bool isOnBottom = room.blocks.cells.GetRelativeRoomCellCoordinates(roomCell).floor == 0;

            // TODO - account for room width being less than MAX_OVERHANG
            int roomWidth = room.blocks.cells.coordinatesList.width;

            if (isOnBottom && roomCell.coordinates.floor > 0)
            {
                Room roomUnderneath = appState.Rooms.queries.FindRoomAtCell(new CellCoordinates(roomCell.coordinates.x, roomCell.coordinates.floor - 1));

                if (roomUnderneath == null)
                {
                    // cell is overhanging - look for rooms underneath within acceptable overhange range
                    CellCoordinates cellUnderneathToTheLeft = new CellCoordinates(roomCell.coordinates.x - 1, roomCell.coordinates.floor - 1);
                    Room roomUnderneathToTheLeft = appState.Rooms.queries.FindRoomAtCell(cellUnderneathToTheLeft);

                    CellCoordinates cellUnderneathToTheRight = new CellCoordinates(roomCell.coordinates.x + 1, roomCell.coordinates.floor - 1);
                    Room roomUnderneathToTheRight = appState.Rooms.queries.FindRoomAtCell(cellUnderneathToTheRight);

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

        public static List<RoomValidationError> ValidateRoomCellIsNotUnderground(AppState appState, Room room, RoomCell roomCell)
        {
            if (roomCell.coordinates.floor < 0)
            {
                return Helpers.CreateErrorList("You cannot build rooms underground.");
            }

            return Helpers.CreateEmptyErrorList();
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
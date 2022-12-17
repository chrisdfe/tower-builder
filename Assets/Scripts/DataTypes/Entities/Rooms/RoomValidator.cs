using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms.Validators
{
    public class RoomValidator : EntityValidator
    {
        protected override List<EntityCellValidationFunc> cellValidators { get; } =
            new List<EntityCellValidationFunc>()
            {
                ValidateAboveOtherRoom,
                ValidateAcceptableOverhang,
            };

        public RoomValidator(Room room) : base(room) { }

        public static EntityValidationErrorList ValidateAboveOtherRoom(AppState appState, Entity entity, CellCoordinates cellCoordinates)
        {
            // It's fine to be built on the ground floor
            if (entity.cellCoordinatesList.lowestFloor == 0)
            {
                return new EntityValidationErrorList();
            }

            CellCoordinatesList bottomRow = entity.cellCoordinatesList.bottomRow;

            foreach (CellCoordinates bottomRowCellCoordinates in bottomRow.items)
            {
                Room roomBelow = appState.Entities.Rooms.queries
                    .FindEntityTypeAtCell(cellCoordinates.coordinatesBelow);

                if (roomBelow == null)
                {
                    return new EntityValidationErrorList("Room must be built above other room");
                }
            }

            return new EntityValidationErrorList();
        }

        public static EntityValidationErrorList ValidateAcceptableOverhang(AppState appState, Entity entity, CellCoordinates cellCoordinates)
        {
            Room room = entity as Room;
            bool isOnBottom = room.cellCoordinatesList.lowestFloor == 0;

            // TODO - account for room width being less than MAX_OVERHANG
            int roomWidth = room.cellCoordinatesList.width;

            if (isOnBottom && cellCoordinates.floor > 0)
            {
                Room roomUnderneath = appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelow);

                if (roomUnderneath == null)
                {
                    // cell is overhanging - look for rooms underneath within acceptable overhang range
                    Room roomUnderneathToTheLeft = appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelowLeft);
                    Room roomUnderneathToTheRight = appState.Entities.Rooms.queries.FindRoomAtCell(cellCoordinates.coordinatesBelowRight);

                    if (roomUnderneathToTheLeft == null && roomUnderneathToTheRight == null)
                    {
                        return new EntityValidationErrorList($"Rooms must have a maximum overhang of 1 cell.");
                    }
                }

            }

            return new EntityValidationErrorList();
        }

    }
}
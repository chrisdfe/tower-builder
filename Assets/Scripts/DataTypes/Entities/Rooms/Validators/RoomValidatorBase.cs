using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms.Validators
{
    public class RoomValidator : EntityValidator
    {
        protected override List<EntityCellValidationFunc> cellValidators { get; } =
            EntityValidator.BaseCellValidators.Concat(new List<EntityCellValidationFunc>()
            {
                ValidateAcceptableOverhang,
            }).ToList();

        public RoomValidator(Room room) : base(room) { }

        public static EntityValidationErrorList ValidateAcceptableOverhang(AppState appState, Entity entity, CellCoordinates cellCoordinates)
        {
            Room room = entity as Room;
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
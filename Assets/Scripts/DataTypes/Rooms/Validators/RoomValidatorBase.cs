using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public delegate List<RoomValidationError> RoomValidationFunc(AppState appState, Room room);

    public delegate List<RoomValidationError> RoomCellValidationFunc(AppState appState, Room room, RoomCell roomCell);

    public abstract class RoomValidatorBase : ValidatorBase<RoomValidationError>
    {
        Room room;

        protected virtual List<RoomValidationFunc> roomValidators { get; } = new List<RoomValidationFunc>();
        protected virtual List<RoomCellValidationFunc> roomCellValidators { get; } = new List<RoomCellValidationFunc>();

        // Room Validators that get run on every room
        List<RoomValidationFunc> baseRoomValidators { get; } = new List<RoomValidationFunc>() {
          GenericRoomValidations.ValidateWallet
        };

        List<RoomCellValidationFunc> baseRoomCellValidators { get; } = new List<RoomCellValidationFunc>() {
          GenericRoomCellValidations.ValidateRoomCellIsNotOverlappingAnotherRoom
        };

        public RoomValidatorBase(Room room)
        {
            this.room = room;
        }

        public override void Validate(AppState appState)
        {
            errors = new List<RoomValidationError>();

            List<RoomValidationFunc> AllRoomValidators = baseRoomValidators.Concat(roomValidators).ToList();
            List<RoomCellValidationFunc> AllRoomCellValidators = baseRoomCellValidators.Concat(roomCellValidators).ToList();

            foreach (RoomValidationFunc RoomValidationFunc in AllRoomValidators)
            {
                errors = errors.Concat(RoomValidationFunc(appState, room)).ToList();
            }

            foreach (RoomCell roomCell in room.blocks.cells.cells)
            {
                foreach (RoomCellValidationFunc RoomCellValidationFunc in AllRoomCellValidators)
                {
                    errors = errors.Concat(RoomCellValidationFunc(appState, room, roomCell)).ToList();
                }
            }
        }
    }
}
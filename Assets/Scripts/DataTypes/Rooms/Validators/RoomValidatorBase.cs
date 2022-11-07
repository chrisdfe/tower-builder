using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public class RoomValidatorBase
    {
        Room room;

        public List<RoomValidationError> errors { get; private set; } = new List<RoomValidationError>();

        public bool isValid { get { return errors.Count == 0; } }

        protected virtual List<GenericRoomValidations.ValidationFunc> RoomValidators { get; } = new List<GenericRoomValidations.ValidationFunc>();
        protected virtual List<GenericRoomCellValidations.ValidationFunc> RoomCellValidators { get; } = new List<GenericRoomCellValidations.ValidationFunc>();

        // Room Validators that get run on every room
        List<GenericRoomValidations.ValidationFunc> BaseRoomValidators { get; } = new List<GenericRoomValidations.ValidationFunc>() {
          GenericRoomValidations.ValidateWallet
        };

        List<GenericRoomCellValidations.ValidationFunc> BaseRoomCellValidators { get; } = new List<GenericRoomCellValidations.ValidationFunc>() {
          GenericRoomCellValidations.ValidateOverlap
        };

        public RoomValidatorBase(Room room)
        {
            this.room = room;
        }

        public void Validate(AppState appState)
        {
            errors = new List<RoomValidationError>();

            List<GenericRoomValidations.ValidationFunc> AllRoomValidators = RoomValidators.Concat(BaseRoomValidators).ToList();
            List<GenericRoomCellValidations.ValidationFunc> AllRoomCellValidators = RoomCellValidators.Concat(BaseRoomCellValidators).ToList();

            foreach (GenericRoomValidations.ValidationFunc RoomValidationFunc in AllRoomValidators)
            {
                errors = errors.Concat(RoomValidationFunc(appState, room)).ToList();
            }

            foreach (RoomCell roomCell in room.blocks.cells.cells)
            {
                foreach (GenericRoomCellValidations.ValidationFunc RoomCellValidationFunc in AllRoomCellValidators)
                {
                    errors = errors.Concat(RoomCellValidationFunc(appState, room, roomCell)).ToList();
                }
            }
        }
    }
}
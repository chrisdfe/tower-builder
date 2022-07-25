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
        public virtual List<RoomValidationError> Validate(Room room, AppState stores)
        {
            List<RoomValidationError> result = new List<RoomValidationError>();
            result = result.Concat(ValidateRoom(room, stores)).ToList();

            foreach (RoomCell roomCell in room.cells.items)
            {
                result = result.Concat(ValidateRoomCell(roomCell, stores)).ToList();
            }

            return result;
        }

        public virtual List<RoomValidationError> ValidateRoom(Room room, AppState stores)
        {
            List<RoomValidationError> result = new List<RoomValidationError>();

            int walletBalance = stores.Wallet.balance;

            if (walletBalance < room.price)
            {
                result.Add(new RoomValidationError("Insufficient Funds."));
            }

            return result;
        }

        public virtual List<RoomValidationError> ValidateRoomCell(RoomCell roomCell, AppState stores)
        {
            RoomList allRooms = stores.Rooms.roomList;
            List<RoomValidationError> result = new List<RoomValidationError>();

            // Check for overlapping cells
            foreach (Room otherRoom in allRooms.items)
            {
                if (otherRoom.cells.Contains(roomCell))
                {
                    result.Add(
                        new RoomValidationError("You cannot build rooms on top of each other.")
                    );
                }
            }

            return result;
        }
    }
}
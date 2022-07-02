using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using UnityEngine;

namespace TowerBuilder.State.Rooms.Validators
{
    public class RoomValidatorBase
    {
        public virtual List<RoomValidationError> Validate(Room room, AppState stores)
        {
            List<RoomValidationError> result = new List<RoomValidationError>();
            result = result.Concat(ValidateRoom(room, stores)).ToList();

            foreach (RoomCell roomCell in room.roomCells.cells)
            {
                result = result.Concat(ValidateRoomCell(roomCell, stores)).ToList();
            }

            return result;
        }

        public virtual List<RoomValidationError> ValidateRoom(Room room, AppState stores)
        {
            List<RoomValidationError> result = new List<RoomValidationError>();

            int walletBalance = stores.Wallet.balance;

            if (walletBalance < room.roomTemplate.price)
            {
                result.Add(new RoomValidationError("Insufficient Funds."));
            }

            return result;
        }

        public virtual List<RoomValidationError> ValidateRoomCell(RoomCell roomCell, AppState stores)
        {
            RoomList allRooms = stores.Rooms.buildings.FindAllRooms();
            List<RoomValidationError> result = new List<RoomValidationError>();

            // Check for overlapping cells
            foreach (Room otherRoom in allRooms.rooms)
            {
                if (otherRoom.roomCells.Contains(roomCell))
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Rooms.Validators
{
    public class RoomValidatorBase
    {
        public virtual List<RoomValidationError> Validate(Room room, StoreRegistry stores)
        {
            List<RoomValidationError> result = new List<RoomValidationError>();
            result = result.Concat(ValidateRoom(room, stores)).ToList();

            foreach (RoomCell roomCell in room.roomCells.cells)
            {
                result = result.Concat(ValidateRoomCell(roomCell, stores)).ToList();
            }

            return result;
        }

        public virtual List<RoomValidationError> ValidateRoom(Room room, StoreRegistry stores)
        {
            List<RoomValidationError> result = new List<RoomValidationError>();

            int walletBalance = stores.Wallet.balance;

            if (walletBalance < room.roomTemplate.price)
            {
                result.Add(new RoomValidationError("Insufficient Funds."));
            }

            return result;
        }

        public virtual List<RoomValidationError> ValidateRoomCell(RoomCell roomCell, StoreRegistry stores)
        {
            RoomList allRooms = stores.Rooms.rooms;
            List<RoomValidationError> result = new List<RoomValidationError>();

            // Check for overlapping cells
            foreach (Room room in allRooms.rooms)
            {
                foreach (RoomCell otherRoomCell in room.roomCells.cells)
                {
                    if (otherRoomCell.coordinates.Matches(roomCell.coordinates))
                    {
                        result.Add(
                            new RoomValidationError("You cannot build rooms on top of each other.")
                        );
                    }
                }
            }

            return result;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public class RoomBlueprint
    {
        public CellCoordinates cellCoordinates;
        public RoomKey roomKey;

        public RoomCells roomCells { get; private set; }
        public List<RoomBlueprintCell> roomBlueprintCells;
        public List<RoomBlueprintValidationError> validationErrors { get; private set; }

        public RoomBlueprint(CellCoordinates cellCoordinates, RoomKey roomKey)
        {
            this.cellCoordinates = cellCoordinates;
            this.roomKey = roomKey;
            validationErrors = new List<RoomBlueprintValidationError>();

            UpdateRoomCells();
        }

        public void SetRoomKey(RoomKey roomKey)
        {
            this.roomKey = roomKey;
            UpdateRoomCells();
        }

        public void SetCellCoordinates(CellCoordinates cellCoordinates)
        {
            this.cellCoordinates = cellCoordinates;
            UpdateRoomCells();
        }

        public RoomCells GetPositionedRoomCells()
        {
            return RoomCells.PositionAtCoordinates(roomCells, cellCoordinates);
        }

        public void Validate(List<MapRoom> mapRooms, int walletBalance)
        {
            ValidateTopLevel(walletBalance);
            ValidateRoomCells(mapRooms);
        }

        public List<RoomBlueprintValidationError> GetAllValidationErrors()
        {
            List<RoomBlueprintValidationError> cellValidationErrors = GetCellValidationErrors();
            return validationErrors.Concat(cellValidationErrors).ToList();
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }

        // Top-level validations, like whether the player has enough money
        // These apply to all cells in this blueprint (they will all be red)
        void ValidateTopLevel(int walletBalance)
        {
            validationErrors = new List<RoomBlueprintValidationError>();

            RoomDetails roomDetails = TowerBuilder.Stores.Rooms.Constants.ROOM_DETAILS_MAP[roomKey];

            if (walletBalance < roomDetails.price)
            {
                validationErrors.Add(new RoomBlueprintValidationError("Insufficient Funds."));
            }
        }

        // Per-cell level validations
        // like overlapping tiles
        void ValidateRoomCells(List<MapRoom> mapRooms)
        {
            foreach (RoomBlueprintCell roomBlueprintCell in roomBlueprintCells)
            {
                roomBlueprintCell.Validate(mapRooms);
            }
        }

        List<RoomBlueprintValidationError> GetCellValidationErrors()
        {
            List<RoomBlueprintValidationError> result = new List<RoomBlueprintValidationError>();

            foreach (RoomBlueprintCell roomBlueprintCell in roomBlueprintCells)
            {
                foreach (RoomBlueprintValidationError validationError in roomBlueprintCell.validationErrors)
                {
                    result.Add(validationError);
                }
            }

            return result;
        }

        void UpdateRoomCells()
        {
            // TODO - clean up existing roomBlueprintCells?

            // TODO - not this
            if (roomKey == RoomKey.None)
            {
                roomCells = RoomCells.CreateRectangularRoom(0, 0);
            }
            else if (roomKey == RoomKey.Condo)
            {
                roomCells = RoomCells.CreateRectangularRoom(4, 1);
            }
            else if (roomKey == RoomKey.Office)
            {
                roomCells = RoomCells.CreateRectangularRoom(3, 2);
            }
            else
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1);
            }

            roomBlueprintCells = new List<RoomBlueprintCell>();
            foreach (CellCoordinates roomCellCoordinates in roomCells.cells)
            {
                RoomBlueprintCell newRoomBlueprintCell = new RoomBlueprintCell(this, roomCellCoordinates);
                roomBlueprintCells.Add(newRoomBlueprintCell);
            }
        }
    }
}



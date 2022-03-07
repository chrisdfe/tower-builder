using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public class RoomBlueprint
    {
        public List<RoomBlueprintCell> roomBlueprintCells;
        public List<RoomBlueprintValidationError> validationErrors { get; private set; }

        public RoomKey roomKey { get; private set; }

        public CellCoordinates buildStartCoordinates { get; private set; }
        public CellCoordinates buildEndCoordinates { get; private set; }

        public RoomBlueprint(CellCoordinates buildStartCoordinates, RoomKey roomKey)
        {
            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();
            this.roomKey = roomKey;

            validationErrors = new List<RoomBlueprintValidationError>();

            ResetRoomBlueprintCells();
        }

        public void SetRoomKey(RoomKey roomKey)
        {
            this.roomKey = roomKey;
            ResetRoomBlueprintCells();
        }

        public void SetBuildEndCell(CellCoordinates buildEndCoordinates)
        {
            this.buildEndCoordinates = buildEndCoordinates;
            ResetRoomBlueprintCells();
        }

        public void SetBuildStartCell(CellCoordinates buildStartCoordinates)
        {
            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();

            ResetRoomBlueprintCells();
        }

        public List<CellCoordinates> GetRoomCells()
        {
            List<CellCoordinates> roomCells = new List<CellCoordinates>();

            foreach (RoomBlueprintCell roomBlueprintCell in roomBlueprintCells)
            {
                roomCells.Add(roomBlueprintCell.cellCoordinates);
            }

            // return RoomCells.PositionAtCoordinates(roomCells, buildEndCoordinates);
            return roomCells;
        }

        public List<RoomBlueprintValidationError> Validate(List<Room> rooms, int walletBalance)
        {
            List<RoomBlueprintValidationError> topLevelValidationErrors = GetTopLevelValidationErrors(walletBalance);
            List<RoomBlueprintValidationError> cellValidationErrors = GetCellValidationErrors(rooms);
            validationErrors = topLevelValidationErrors.Concat(cellValidationErrors).ToList();
            return validationErrors;
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }

        // Top-level validations, like whether the player has enough money
        // These apply to all cells in this blueprint (they will all be red)
        List<RoomBlueprintValidationError> GetTopLevelValidationErrors(int walletBalance)
        {
            List<RoomBlueprintValidationError> result = new List<RoomBlueprintValidationError>();

            MapRoomDetails roomDetails = TowerBuilder.Stores.Map.Constants.ROOM_DETAILS_MAP[roomKey];

            if (walletBalance < roomDetails.price)
            {
                validationErrors.Add(new RoomBlueprintValidationError("Insufficient Funds."));
            }

            return result;
        }

        // Per-cell level validations
        // like overlapping tiles
        // TODO - room-specific validations
        List<RoomBlueprintValidationError> GetCellValidationErrors(List<Room> rooms)
        {
            List<RoomBlueprintValidationError> result = new List<RoomBlueprintValidationError>();

            foreach (RoomBlueprintCell roomBlueprintCell in roomBlueprintCells)
            {
                roomBlueprintCell.Validate(rooms);

                foreach (RoomBlueprintValidationError validationError in roomBlueprintCell.validationErrors)
                {
                    result.Add(validationError);
                }
            }

            return result;
        }

        void ResetRoomBlueprintCells()
        {
            // TODO - clean up existing roomBlueprintCells first?
            List<CellCoordinates> roomCells = new List<CellCoordinates>();

            MapRoomDetails mapRoomDetails = Stores.Map.Constants.ROOM_DETAILS_MAP[roomKey];

            if (mapRoomDetails.roomResizability.Matches(RoomResizability.Inflexible()))
            {
                roomCells = RoomCells.PositionAtCoordinates(mapRoomDetails.roomCells, buildStartCoordinates);
            }
            else
            {
                CellCoordinates flexibleBuildEndCoordinates = buildStartCoordinates.Clone();

                if (mapRoomDetails.roomResizability.x && buildEndCoordinates.x != buildStartCoordinates.x)
                {
                    flexibleBuildEndCoordinates.x = buildEndCoordinates.x;
                }

                if (mapRoomDetails.roomResizability.floor && buildEndCoordinates.floor != buildStartCoordinates.floor)
                {
                    flexibleBuildEndCoordinates.floor = buildEndCoordinates.floor;
                }

                // Draw a rectangle between the build start coordinates and build end coordinates
                roomCells = RoomCells.CreateRectangularRoom(buildStartCoordinates, flexibleBuildEndCoordinates);
            }

            roomBlueprintCells = new List<RoomBlueprintCell>();
            foreach (CellCoordinates roomCellCoordinates in roomCells)
            {
                RoomBlueprintCell newRoomBlueprintCell = new RoomBlueprintCell(this, roomCellCoordinates);
                roomBlueprintCells.Add(newRoomBlueprintCell);
            }
        }
    }
}



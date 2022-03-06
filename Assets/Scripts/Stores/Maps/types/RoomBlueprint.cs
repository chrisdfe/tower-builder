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
        // public bool buildIsActive { get; private set; } = false;

        public RoomBlueprint(CellCoordinates buildStartCoordinates, RoomKey roomKey)
        {
            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();
            this.roomKey = roomKey;

            validationErrors = new List<RoomBlueprintValidationError>();

            UpdateRoomBlueprintCells();
        }

        public void SetRoomKey(RoomKey roomKey)
        {
            this.roomKey = roomKey;
            UpdateRoomBlueprintCells();
        }

        public void SetBuildEndCell(CellCoordinates buildEndCoordinates)
        {
            this.buildEndCoordinates = buildEndCoordinates;
            UpdateRoomBlueprintCells();
        }

        public void SetBuildStartCell(CellCoordinates buildStartCoordinates)
        {
            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();
            // this.buildIsActive = true;
            UpdateRoomBlueprintCells();
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

        public void Validate(List<Room> rooms, int walletBalance)
        {
            ValidateTopLevel(walletBalance);
            ValidateRoomCells(rooms);
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

            MapRoomDetails roomDetails = TowerBuilder.Stores.Map.Constants.ROOM_DETAILS_MAP[roomKey];

            if (walletBalance < roomDetails.price)
            {
                validationErrors.Add(new RoomBlueprintValidationError("Insufficient Funds."));
            }
        }

        // Per-cell level validations
        // like overlapping tiles
        // TODO - room-specific validations
        void ValidateRoomCells(List<Room> rooms)
        {
            foreach (RoomBlueprintCell roomBlueprintCell in roomBlueprintCells)
            {
                roomBlueprintCell.Validate(rooms);
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

        void UpdateRoomBlueprintCells()
        {
            // TODO - clean up existing roomBlueprintCells first?
            List<CellCoordinates> roomCells = RoomCells.CreateRectangularRoom(0, 0);

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



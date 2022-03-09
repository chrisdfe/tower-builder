using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Blueprints
{
    public class Blueprint
    {
        public List<BlueprintCell> roomBlueprintCells;
        public List<BlueprintValidationError> validationErrors { get; private set; }

        public RoomKey roomKey { get; private set; }

        public CellCoordinates buildStartCoordinates { get; private set; }
        public CellCoordinates buildEndCoordinates { get; private set; }

        public Blueprint(CellCoordinates buildStartCoordinates, RoomKey roomKey)
        {
            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();
            this.roomKey = roomKey;

            validationErrors = new List<BlueprintValidationError>();

            ResetBlueprintCells();
        }

        public void SetRoomKey(RoomKey roomKey)
        {
            this.roomKey = roomKey;
            ResetBlueprintCells();
        }

        public void SetBuildEndCell(CellCoordinates buildEndCoordinates)
        {
            this.buildEndCoordinates = buildEndCoordinates;
            ResetBlueprintCells();
        }

        public void SetBuildStartCell(CellCoordinates buildStartCoordinates)
        {
            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();

            ResetBlueprintCells();
        }

        public RoomCells GetRoomCells()
        {
            RoomCells roomCells = new RoomCells();

            foreach (BlueprintCell roomBlueprintCell in roomBlueprintCells)
            {
                roomCells.Add(roomBlueprintCell.cellCoordinates);
            }

            return roomCells;
        }

        public List<BlueprintValidationError> Validate(RoomList rooms, int walletBalance)
        {
            List<BlueprintValidationError> topLevelValidationErrors = GetTopLevelValidationErrors(walletBalance);
            List<BlueprintValidationError> cellValidationErrors = GetCellValidationErrors(rooms);
            validationErrors = topLevelValidationErrors.Concat(cellValidationErrors).ToList();
            return validationErrors;
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }

        // Top-level validations, like whether the player has enough money
        // These apply to all cells in this blueprint (they will all be red)
        List<BlueprintValidationError> GetTopLevelValidationErrors(int walletBalance)
        {
            List<BlueprintValidationError> result = new List<BlueprintValidationError>();

            RoomDetails roomDetails = Room.GetDetails(roomKey);

            if (walletBalance < roomDetails.price)
            {
                validationErrors.Add(new BlueprintValidationError("Insufficient Funds."));
            }

            return result;
        }

        // Per-cell level validations
        // like overlapping tiles
        // TODO - room-specific validations
        List<BlueprintValidationError> GetCellValidationErrors(RoomList roomList)
        {
            List<BlueprintValidationError> result = new List<BlueprintValidationError>();

            foreach (BlueprintCell roomBlueprintCell in roomBlueprintCells)
            {
                roomBlueprintCell.Validate(roomList);

                foreach (BlueprintValidationError validationError in roomBlueprintCell.validationErrors)
                {
                    result.Add(validationError);
                }
            }

            return result;
        }

        void ResetBlueprintCells()
        {
            // TODO - clean up existing roomBlueprintCells first?
            List<CellCoordinates> roomCells = new List<CellCoordinates>();

            RoomDetails roomDetails = Room.GetDetails(roomKey);

            if (roomDetails.roomResizability.Matches(RoomResizability.Inflexible()))
            {
                // TODO - use instance method instead
                roomCells = RoomCells.PositionAtCoordinates(
                    new RoomCells(roomDetails.width, roomDetails.height).cells,
                    buildStartCoordinates
                );
            }
            else
            {
                CellCoordinates flexibleBuildEndCoordinates = buildStartCoordinates.Clone();

                if (roomDetails.roomResizability.x && buildEndCoordinates.x != buildStartCoordinates.x)
                {
                    flexibleBuildEndCoordinates.x = buildEndCoordinates.x;
                }

                if (roomDetails.roomResizability.floor && buildEndCoordinates.floor != buildStartCoordinates.floor)
                {
                    flexibleBuildEndCoordinates.floor = buildEndCoordinates.floor;
                }

                // Draw a rectangle between the build start coordinates and build end coordinates
                roomCells = RoomCells.CreateRectangularRoom(buildStartCoordinates, flexibleBuildEndCoordinates);
            }

            roomBlueprintCells = new List<BlueprintCell>();
            foreach (CellCoordinates roomCellCoordinates in roomCells)
            {
                BlueprintCell newBlueprintCell = new BlueprintCell(this, roomCellCoordinates);
                roomBlueprintCells.Add(newBlueprintCell);
            }
        }
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.Map.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Blueprints
{
    public class Blueprint
    {
        public List<BlueprintCell> roomBlueprintCells;
        public List<RoomValidationError> validationErrors { get; private set; }

        public Room room { get; private set; }

        public CellCoordinates buildStartCoordinates { get; private set; }
        public CellCoordinates buildEndCoordinates { get; private set; }

        public Blueprint(RoomKey roomKey, CellCoordinates buildStartCoordinates)
        {
            this.room = new Room(roomKey);
            this.room.isInBlueprintMode = true;

            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();

            validationErrors = new List<RoomValidationError>();

            ResetBlueprintCells();
        }

        public void OnDestroy() { }

        public void Reset()
        {
            RoomKey roomKey = this.room.roomKey;
            this.room = new Room(roomKey);
        }

        public void SetRoomKey(RoomKey roomKey)
        {
            room.SetRoomKey(roomKey);
            SetRoomCells();
            ResetBlueprintCells();
        }

        public void SetBuildStartCell(CellCoordinates buildStartCoordinates)
        {
            this.buildStartCoordinates = buildStartCoordinates.Clone();
            this.buildEndCoordinates = buildStartCoordinates.Clone();

            ResetBlueprintCells();
        }

        public void SetBuildEndCell(CellCoordinates buildEndCoordinates)
        {
            this.buildEndCoordinates = buildEndCoordinates;
            ResetBlueprintCells();
        }

        public void SetRoomCells()
        {
            RoomDetails roomDetails = room.roomDetails;
            RoomCells roomCells;

            if (roomDetails.resizability.Matches(RoomResizability.Inflexible()))
            {
                roomCells = new RoomCells(roomDetails.width, roomDetails.height);
                roomCells.PositionAtCoordinates(buildStartCoordinates);
            }
            else
            {
                CellCoordinates flexibleBuildStartCoordinates = buildStartCoordinates.Clone();
                CellCoordinates flexibleBuildEndCoordinates = buildStartCoordinates.Clone();

                // Restrict resizability to X/Y depending on RoomFlexibility
                // TODO - it's not super obvious what is going on here
                if (
                    roomDetails.resizability.x &&
                    buildEndCoordinates.x != buildStartCoordinates.x
                )
                {
                    flexibleBuildEndCoordinates.x = buildEndCoordinates.x;
                }

                if (
                    roomDetails.resizability.floor &&
                    buildEndCoordinates.floor != buildStartCoordinates.floor
                )
                {
                    flexibleBuildEndCoordinates.floor = buildEndCoordinates.floor;
                }

                // Make sure that the start coordinates are in the top left
                if (flexibleBuildStartCoordinates.x > flexibleBuildEndCoordinates.x)
                {
                    CellCoordinates temp = flexibleBuildEndCoordinates.Clone();
                    flexibleBuildEndCoordinates = flexibleBuildStartCoordinates.Clone();
                    flexibleBuildStartCoordinates = temp.Clone();
                }

                // Round up to fit base blueprint size
                CellCoordinates selectionAreaDimensions = new CellCoordinates(
                    (flexibleBuildEndCoordinates.x - flexibleBuildStartCoordinates.x) + 1,
                    (flexibleBuildEndCoordinates.floor - flexibleBuildStartCoordinates.floor) + 1
                );

                // Dimensions rounded up to the nearest width or height
                CellCoordinates roundedUpDimensions = new CellCoordinates(
                    (int)(Mathf.Ceil(((float)selectionAreaDimensions.x / (float)roomDetails.width)) * (float)roomDetails.width),
                    (int)(Mathf.Ceil(((float)selectionAreaDimensions.floor / (float)roomDetails.height)) * (float)roomDetails.height)
                );

                // Determine how many copies (number of the base room blueprint size) there are in each direction
                // TODO - it feels like this "get copies"  code could be abstracted out into a new method
                // Flexible room sizes need to know how many of the room blueprint can fit within
                // the start+end coordinates
                CellCoordinates copies = new CellCoordinates(
                    (int)((float)roundedUpDimensions.x / (float)roomDetails.width),
                    (int)((float)roundedUpDimensions.floor / (float)roomDetails.height)
                );

                flexibleBuildEndCoordinates = new CellCoordinates(
                    flexibleBuildStartCoordinates.x + (copies.x * roomDetails.width) - 1,
                    flexibleBuildStartCoordinates.floor + (copies.floor * roomDetails.height) - 1
                );

                roomCells = new RoomCells(flexibleBuildStartCoordinates, flexibleBuildEndCoordinates);
            }

            this.room.SetRoomCells(roomCells);
        }

        public int GetPrice()
        {
            RoomDetails roomDetails = room.roomDetails;

            // Subtract appropriate balance from wallet
            if (roomDetails.resizability.Matches(RoomResizability.Inflexible()))
            {
                return roomDetails.price;
            }

            CellCoordinates copies = GetCopies();
            // Work out how many copies of the base blueprint size is being built
            // roomDetails.price is per base blueprint copy
            // TODO - this calculation should be elsewhere to allow this number to show up in the UI
            //        as the blueprint is being built

            int result = roomDetails.price * copies.x * copies.floor;

            return result;
        }

        public CellCoordinates GetCopies()
        {
            return new CellCoordinates(
                room.roomCells.GetWidth() / room.roomDetails.width,
                room.roomCells.GetFloorSpan() / room.roomDetails.height
            );
        }

        public List<RoomValidationError> Validate(StoreRegistry stores)
        {
            validationErrors = room.roomDetails.validator.Validate(room, stores);
            return validationErrors;
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }

        void ResetBlueprintCells()
        {
            SetRoomCells();

            // TODO - clean up existing roomBlueprintCells first?
            roomBlueprintCells = new List<BlueprintCell>();

            foreach (RoomCell roomCell in room.roomCells.cells)
            {
                BlueprintCell newBlueprintCell = new BlueprintCell(this, roomCell);
                roomBlueprintCells.Add(newBlueprintCell);
            }
        }
    }
}



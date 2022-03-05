using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public class RoomBlueprintCell
    {
        public RoomBlueprint parentBlueprint { get; private set; }

        public string id { get; private set; }
        public CellCoordinates cellCoordinates { get; private set; } = CellCoordinates.zero;

        public CellCoordinates absoluteCellCoordinates
        {
            get
            {
                // return parentBlueprint.buildStartCoordinates)
                return cellCoordinates;
            }
        }

        // relative to parentBlueprint
        public CellCoordinates relativeCellCoordiantes
        {
            get
            {
                return cellCoordinates.Subtract(parentBlueprint.buildStartCoordinates);
            }
        }

        public List<RoomBlueprintValidationError> validationErrors;

        public RoomBlueprintCell(RoomBlueprint parentBlueprint, CellCoordinates cellCoordinates)
        {
            id = Guid.NewGuid().ToString();
            validationErrors = new List<RoomBlueprintValidationError>();
            this.parentBlueprint = parentBlueprint;
            this.cellCoordinates = cellCoordinates;
            UpdateAndValidateCellCoordinates(cellCoordinates);
        }

        public void UpdateAndValidateCellCoordinates(CellCoordinates cellCoordinates)
        {
            this.cellCoordinates = cellCoordinates;
        }

        public void Validate(List<MapRoom> mapRooms)
        {
            validationErrors = new List<RoomBlueprintValidationError>();

            // Check for overlapping cells
            foreach (MapRoom mapRoom in mapRooms)
            {
                foreach (CellCoordinates otherRoomCellCoordinates in mapRoom.roomCells)
                {
                    if (otherRoomCellCoordinates.Matches(absoluteCellCoordinates))
                    {
                        validationErrors.Add(new RoomBlueprintValidationError("You cannot build rooms on top of each other."));
                    }
                }
            }
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }
    }
}

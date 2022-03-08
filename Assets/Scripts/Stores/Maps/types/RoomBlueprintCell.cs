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

        // rooms is a list of all of the rooms on the map currently
        public void Validate(List<Room> allRooms)
        {
            validationErrors = Validators.ValidateBlueprintCell(this, allRooms);
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }
    }
}

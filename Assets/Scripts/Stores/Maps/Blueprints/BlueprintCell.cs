using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Blueprints
{
    public class BlueprintCell
    {
        public Blueprint parentBlueprint { get; private set; }

        public string id { get; private set; }
        public CellCoordinates cellCoordinates { get; private set; } = CellCoordinates.zero;

        public List<BlueprintValidationError> validationErrors;

        public BlueprintCell(Blueprint parentBlueprint, CellCoordinates cellCoordinates)
        {
            id = Guid.NewGuid().ToString();
            validationErrors = new List<BlueprintValidationError>();
            this.parentBlueprint = parentBlueprint;
            this.cellCoordinates = cellCoordinates;
            UpdateAndValidateCellCoordinates(cellCoordinates);
        }

        public void UpdateAndValidateCellCoordinates(CellCoordinates cellCoordinates)
        {
            this.cellCoordinates = cellCoordinates;
        }

        // rooms is a list of all of the rooms on the map currently
        public void Validate(RoomList allRooms)
        {
            validationErrors = BlueprintValidators.ValidateBlueprintCell(this, allRooms);
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }
    }
}

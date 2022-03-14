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
        public string id { get; private set; }

        public Blueprint parentBlueprint { get; private set; }
        public RoomCell roomCell { get; private set; }

        public List<BlueprintValidationError> validationErrors;

        public CellCoordinates relativeCellCoordinates
        {
            get
            {
                return roomCell.coordinates.Subtract(parentBlueprint.buildStartCoordinates);
            }
        }

        public BlueprintCell(Blueprint parentBlueprint, RoomCell roomCell)
        {
            id = Guid.NewGuid().ToString();
            validationErrors = new List<BlueprintValidationError>();
            this.parentBlueprint = parentBlueprint;
            this.roomCell = roomCell;
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

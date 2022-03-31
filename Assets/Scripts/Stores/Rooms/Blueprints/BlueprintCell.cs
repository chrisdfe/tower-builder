using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Validators;
using UnityEngine;

namespace TowerBuilder.Stores.Rooms.Blueprints
{
    public class BlueprintCell
    {
        public string id { get; private set; }

        public Blueprint parentBlueprint { get; private set; }
        public RoomCell roomCell { get; private set; }

        public List<RoomValidationError> validationErrors;

        public BlueprintCell(Blueprint parentBlueprint, RoomCell roomCell)
        {
            id = Guid.NewGuid().ToString();
            validationErrors = new List<RoomValidationError>();
            this.parentBlueprint = parentBlueprint;
            this.roomCell = roomCell;
        }

        public bool IsValid()
        {
            return validationErrors.Count == 0;
        }
    }
}

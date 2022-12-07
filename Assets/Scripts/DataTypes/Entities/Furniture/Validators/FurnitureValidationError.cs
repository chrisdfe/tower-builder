using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures.Validators
{
    public class FurnitureValidationError : ValidationErrorBase
    {
        public FurnitureValidationError() { }
        public FurnitureValidationError(string message) : base(message) { }
    }
}
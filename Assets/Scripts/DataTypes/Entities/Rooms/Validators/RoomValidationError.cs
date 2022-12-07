using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Rooms.Validators
{
    public class RoomValidationError : ValidationErrorBase
    {
        public RoomValidationError() : base() { }
        public RoomValidationError(string message) : base(message) { }
    }
}

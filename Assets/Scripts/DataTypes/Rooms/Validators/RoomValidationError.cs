using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Validators
{
    public class RoomValidationError : ValidationErrorBase
    {
        public RoomValidationError(string message) : base(message) { }
    }
}

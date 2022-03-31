using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.Stores.Rooms.Validators
{
    public class RoomValidationError
    {
        public string message { get; private set; }

        public RoomValidationError(string message)
        {
            this.message = message;
        }
    }
}

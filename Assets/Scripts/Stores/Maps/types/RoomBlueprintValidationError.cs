using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{

    public class RoomBlueprintValidationError
    {
        public string message { get; private set; }

        public RoomBlueprintValidationError(string message)
        {
            this.message = message;
        }
    }
}

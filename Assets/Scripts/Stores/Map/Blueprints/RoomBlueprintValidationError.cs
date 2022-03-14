using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Blueprints
{
    public class BlueprintValidationError
    {
        public string message { get; private set; }

        public BlueprintValidationError(string message)
        {
            this.message = message;
        }
    }
}

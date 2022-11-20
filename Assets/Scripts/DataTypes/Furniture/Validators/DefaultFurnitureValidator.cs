using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public class DefaultFurnitureValidator : FurnitureValidatorBase
    {
        public DefaultFurnitureValidator(Furniture furniture) : base(furniture) { }

        public override void Validate(AppState appState) { }
    }
}
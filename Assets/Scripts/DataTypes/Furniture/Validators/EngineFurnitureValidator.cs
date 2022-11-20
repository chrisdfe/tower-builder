using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public class EngineFurnitureValidator : FurnitureValidatorBase
    {
        public EngineFurnitureValidator(Furniture furniture) : base(furniture)
        {
            Debug.Log("New engine furniture validator");
        }

        public override void Validate(AppState appState) { }
    }
}
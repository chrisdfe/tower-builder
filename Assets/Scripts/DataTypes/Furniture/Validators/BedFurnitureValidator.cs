using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public class BedFurnitureValidator : FurnitureValidatorBase
    {
        public BedFurnitureValidator(Furniture furniture) : base(furniture)
        {
            Debug.Log("New Bed furniture validator");
        }
        public override void Validate(AppState appState) { }
    }
}
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public class CockpitFurnitureValidator : FurnitureValidatorBase
    {
        public CockpitFurnitureValidator(Furniture furniture) : base(furniture)
        {
            Debug.Log("New cockpit furniture validator");
        }
    }
}
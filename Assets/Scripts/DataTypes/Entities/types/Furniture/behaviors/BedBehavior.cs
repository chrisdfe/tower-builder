using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class BedBehavior : FurnitureBehavior
    {
        public override Key key => FurnitureBehavior.Key.Bed;

        public BedBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        Resident resident;
        AttributeModifier modifier;

        protected override void OnInteractStart(Resident resident)
        {
            this.resident = resident;
            modifier = new AttributeModifier("sleeping", 1.6f);
            resident.attributes.AddTickAttributeModifier("energy", modifier);
        }

        protected override void OnInteractEnd(Resident resident)
        {
            resident.attributes.RemoveTickAttributeModifier("energy", modifier);
            modifier = null;
        }
    }
}
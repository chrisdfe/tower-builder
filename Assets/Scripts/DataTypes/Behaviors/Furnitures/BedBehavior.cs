using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Attributes.Residents;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Behaviors.Furnitures
{
    public class BedBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Bed;

        public override FurnitureBehaviorTag[] tags { get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Sleeping }; } }

        public BedBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        ResidentAttribute.Modifier modifier;

        public override void InteractStart(Resident resident)
        {
            base.InteractStart(resident);
            ResidentAttributesWrapper residentAttributesWrapper = appState.Attributes.Residents.queries.FindByResident(resident);
            modifier = new ResidentAttribute.Modifier("sleeping", 1.6f);
            appState.Attributes.Residents.AddTickAttributeModifier(residentAttributesWrapper, ResidentAttribute.Key.Energy, modifier);
        }

        public override void InteractEnd(Resident resident)
        {
            base.InteractEnd(resident);
            ResidentAttributesWrapper residentAttributesWrapper = appState.Attributes.Residents.queries.FindByResident(resident);
            appState.Attributes.Residents.RemoveTickAttributeModifier(residentAttributesWrapper, ResidentAttribute.Key.Energy, modifier);
            modifier = null;
        }
    }
}
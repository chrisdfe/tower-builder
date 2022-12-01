using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Residents.Attributes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public class BedBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Bed;

        public override FurnitureBehaviorTag[] tags { get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Sleeping }; } }

        public BedBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        ResidentAttribute.Modifier modifier;

        public override void InteractStart(Resident resident)
        {
            Debug.Log("Bed behavior start");
            base.InteractStart(resident);
            ResidentAttributesWrapper residentAttributesWrapper = appState.ResidentAttributesWrappers.queries.FindByResident(resident);
            modifier = new ResidentAttribute.Modifier("sleeping", 1.6f);
            appState.ResidentAttributesWrappers.AddTickAttributeModifier(resident, ResidentAttribute.Key.Energy, modifier);
        }

        public override void InteractEnd(Resident resident)
        {
            Debug.Log("Bed behavior end");
            base.InteractEnd(resident);
            appState.ResidentAttributesWrappers.RemoveTickAttributeModifier(resident, ResidentAttribute.Key.Energy, modifier);
            modifier = null;
        }
    }
}
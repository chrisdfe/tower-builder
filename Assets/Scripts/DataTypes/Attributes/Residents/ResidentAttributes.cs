using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Attributes.Residents
{
    public class ResidentAttributes : AttributesGroup<ResidentAttributes.Key>
    {
        public enum Key
        {
            Energy
        }

        public Resident resident { get; private set; }

        public override Dictionary<Key, Attribute> attributes { get; } = new Dictionary<Key, Attribute>() {
            {
                Key.Energy,
                new Attribute(100)
                // TODO - add Attribute.Input class to make passing modifiers into constructor less cumbersome
                //  .AddTickModifier(new FloatAttributeModifier("Natural degredation", -0.8f))
            }
        };

        public ResidentAttributes(AppState appState, Resident resident) : base(appState)
        {
            this.resident = resident;
        }
    }
}
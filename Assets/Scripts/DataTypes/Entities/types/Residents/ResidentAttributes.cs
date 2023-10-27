using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class ResidentAttributes : AttributesGroup
    {

        public Resident resident { get; private set; }

        public override Dictionary<string, Attribute> attributes { get; } = new Dictionary<string, Attribute>() {
            { "energy", new Attribute(100) }
                // TODO - add Attribute.Input class to make passing modifiers into constructor less cumbersome
                //  .AddTickModifier(new FloatAttributeModifier("Natural degredation", -0.8f))
        };

        public ResidentAttributes(Resident resident) : base()
        {
            this.resident = resident;
        }
    }
}
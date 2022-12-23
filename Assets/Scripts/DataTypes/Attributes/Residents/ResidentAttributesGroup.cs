using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Attributes.Residents
{
    public class ResidentAttributesGroup : AttributesGroup<ResidentAttribute, ResidentAttribute.Key>
    {
        public Resident resident { get; private set; }

        public override List<ResidentAttribute> attributes { get; } = new List<ResidentAttribute>() {
            new ResidentAttribute(ResidentAttribute.Key.Energy,
                new List<ResidentAttribute.Modifier>(),
                new List<ResidentAttribute.Modifier>() {
                    new ResidentAttribute.Modifier("Natural degredation", -0.8f)
                }
            )
        };

        public ResidentAttributesGroup(AppState appState, Resident resident) : base(appState)
        {
            this.resident = resident;
        }
    }
}
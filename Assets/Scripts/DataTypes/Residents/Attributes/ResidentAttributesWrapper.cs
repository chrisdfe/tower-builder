using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Attributes
{
    public class ResidentAttributesWrapper
    {
        public Resident resident { get; private set; }

        public List<ResidentAttribute> attributes { get; } = new List<ResidentAttribute>() {
            new ResidentAttribute(ResidentAttribute.Key.Energy, 0, 100, 100,
                new List<ResidentAttribute.Modifier>(),
                new List<ResidentAttribute.Modifier>() {
                    new ResidentAttribute.Modifier("Natural degredation", -0.8f)
                }
            )
        };

        public ResidentAttributesWrapper(Resident resident)
        {
            this.resident = resident;
        }

        public void Setup() { }

        public void Teardown() { }

        public ResidentAttribute FindByKey(ResidentAttribute.Key key) => attributes.Find(attribute => attribute.key == key);
    }
}
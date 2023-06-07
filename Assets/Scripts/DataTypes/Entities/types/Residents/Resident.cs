using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class Resident : Entity
    {
        public override string idKey => "Residents";

        public Resident(ResidentDefinition definition) : base(definition) { }

        public override string ToString() => $"Resident {id}";
    }
}
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

        public override string ToString() => $"Resident {id}";

        public ResidentBehavior behavior { get; private set; }
        public ResidentAttributes attributes { get; private set; }

        // TODO - don't add behavior until not in blueprint mode
        public Resident(Input input) : base(input)
        {
            behavior = new ResidentBehavior(this);
            attributes = new ResidentAttributes(this);
        }

        public Resident(ResidentDefinition definition) : base(definition)
        {
            isInBlueprintMode = true;
            behavior = new ResidentBehavior(this);
            attributes = new ResidentAttributes(this);
        }

        public override void OnBuild()
        {
            base.OnBuild();

            behavior.SetEnabled(true);
        }
    }
}
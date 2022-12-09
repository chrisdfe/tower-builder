using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class Resident : Entity<Resident.Key>
    {
        public enum Key
        {
            Default,
        }

        public override string idKey => "Residents";

        public CellCoordinates cellCoordinates;

        public Resident(ResidentDefinition definition) : base(definition)
        {
        }

        // for now allow an empty constructor since resident definitions aren't really used
        public Resident() : this(new ResidentDefinition()) { }

        public override string ToString()
        {
            return $"Resident {id}";
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Residents
{
    public class Resident
    {
        public int id { get; private set; }

        public CellCoordinates cellCoordinates;

        public bool isInBlueprintMode = false;

        public Resident()
        {
            id = UIDGenerator.Generate("resident");
        }

        public override string ToString()
        {
            return $"Resident {id}";
        }

        public void OnBuild()
        {
            this.isInBlueprintMode = false;
        }
    }
}
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
        public CellCoordinates cellCoordinates;

        public bool isInBlueprintMode = false;

        public Resident() { }

        public void OnBuild()
        {
            this.isInBlueprintMode = false;
        }
    }
}
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
        public CellCoordinates coordinates;

        public ResidentMotor motor { get; private set; }
        public ResidentMood mood { get; private set; }
        public ResidentNeeds needs { get; private set; }

        public Resident()
        {
            this.motor = new ResidentMotor(this);
            this.mood = new ResidentMood(this);
            this.needs = new ResidentNeeds(this);
        }
    }
}
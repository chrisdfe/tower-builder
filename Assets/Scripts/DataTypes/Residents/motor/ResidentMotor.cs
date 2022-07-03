using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Residents
{
    public class ResidentMotor
    {
        Resident resident;

        public ResidentMotor(Resident resident)
        {
            this.resident = resident;
        }
    }
}
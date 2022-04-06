using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.Stores.Routes;

using UnityEngine;

namespace TowerBuilder.Stores.Residents
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.State.Routes;

using UnityEngine;

namespace TowerBuilder.State.Residents
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
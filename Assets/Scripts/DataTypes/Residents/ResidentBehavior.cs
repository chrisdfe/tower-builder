using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Residents
{
    public enum ResidentBehaviorState
    {
        Idle,
        Walking
    }

    public class ResidentBehavior
    {
        Resident resident;

        public ResidentBehavior(Resident resident)
        {
            this.resident = resident;
        }
    }
}
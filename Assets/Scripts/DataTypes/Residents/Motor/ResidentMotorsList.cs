using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Motors
{
    public class ResidentMotorsList : ListWrapper<ResidentMotor>
    {
        public ResidentMotor FindByResident(Resident resident)
        {
            return items.Find(residentMotor => residentMotor.resident == resident);
        }
    }
}
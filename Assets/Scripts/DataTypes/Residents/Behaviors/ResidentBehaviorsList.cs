using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Routes;

using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class ResidentBehaviorsList : ListWrapper<ResidentBehavior>
    {
        public ResidentBehavior FindByResident(Resident resident)
        {
            return items.Find(residentBehavior => residentBehavior.resident == resident);
        }
    }
}
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
        public ResidentBehaviorsList() : base() { }
        public ResidentBehaviorsList(ResidentBehavior residentBehaviors) : base(residentBehaviors) { }
        public ResidentBehaviorsList(List<ResidentBehavior> residentBehaviors) : base(residentBehaviors) { }
        public ResidentBehaviorsList(ResidentBehaviorsList residentBehaviorsList) : base(residentBehaviorsList) { }

        public ResidentBehavior FindByResident(Resident resident)
        {
            return items.Find(residentBehavior => residentBehavior.resident == resident);
        }
    }
}